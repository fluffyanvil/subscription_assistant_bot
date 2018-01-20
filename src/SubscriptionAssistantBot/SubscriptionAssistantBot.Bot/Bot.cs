using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SubscriptionAssistantBot.Bot.Builders;
using SubscriptionAssistantBot.Db.Model;
using SubscriptionAssistantBot.Db.Repositories;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace SubscriptionAssistantBot.Bot
{
    public class Bot
    {
        private TelegramBotClient _client;
        private User _botObject;
        private int _botLastGroupMessageId, _botLastPrivateMessageId;
        private IRepository<Tag> TagRepository => _tagRepository ?? (_tagRepository = new TagRepository());
        private IRepository<Tag> _tagRepository;

        private IRepository<Subscription> SubscriptionRepository =>
            _subscriptionRepository ?? (_subscriptionRepository = new SubscriptionRepository());

        private IRepository<Subscription> _subscriptionRepository;

        private InlineKeyboardBuilder _inlineKeyboardBuilder;

        private InlineKeyboardBuilder InlineKeyboardBuilder =>
            _inlineKeyboardBuilder ?? (_inlineKeyboardBuilder = new InlineKeyboardBuilder(_botObject));

        public Bot(string apiToken)
        {
            _client = new TelegramBotClient(apiToken);
            StartBotAsync();
        }

        private async Task StartBotAsync()
        {
            _botObject = await _client.GetMeAsync();
            Console.WriteLine($"Hello I'm {_botObject.Username}");
            _client.OnMessage += ClientOnOnMessage;
            _client.OnMessageEdited += ClientOnOnMessage;
            _client.OnCallbackQuery += ClientOnOnCallbackQuery;
            _client.StartReceiving();
        }

        private async void ClientOnOnCallbackQuery(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var idToDelete = Int32.Parse(callbackQueryEventArgs.CallbackQuery.Data);
            var message = callbackQueryEventArgs.CallbackQuery.Message;
            await SubscriptionRepository.Delete(idToDelete);

            var result = await _client.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id, "Deleted");

            if (result)
            {
                var userSubscriptions = await SubscriptionRepository.FindAll(s => s.UserId.Equals(message.From.Id));
                if (userSubscriptions.Any())
                {
                    var keyboard = InlineKeyboardBuilder.BuildPrivateChatKeyboard(userSubscriptions);
                    await _client.SendTextMessageAsync(message.Chat.Id, "Your subscriptions:", replyMarkup: keyboard);
                }
            }
        }

        private async void ClientOnOnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            if (message.Type != MessageType.TextMessage) return;
            await ProcessIncommingMessage(message);
        }

        private async Task ProcessIncommingMessage(Message message)
        {
            if (message.Chat.Type == ChatType.Group || message.Chat.Type == ChatType.Supergroup)
            {
                await ProcessIncommingGroupMessage(message);
            }

            if (message.Chat.Type == ChatType.Private)
            {
                await ProcessIncommingPrivateMessage(message);
            }
        }

        private async Task ProcessIncommingGroupMessage(Message message)
        {
            var hashtagEntities = message.Entities.Where(e => e.Type.Equals(MessageEntityType.Hashtag));
            var hashtags = hashtagEntities.Select(h => message.Text.Substring(h.Offset, h.Length));
            var existingTags = new List<Tag>();
            foreach (var hashtag in hashtags)
            {
                var existingTag = await TagRepository.Find(t =>
                    t.Value.Equals(hashtag, StringComparison.InvariantCultureIgnoreCase));
                if (existingTag == null)
                {
                    existingTag = await TagRepository.Add(new Tag { Value = hashtag.ToLowerInvariant() });
                }
                existingTags.Add(existingTag);
            }
            var keyboard = InlineKeyboardBuilder.BuildGroupChatKeyboard(existingTags, message.Chat.Id);
            await _client.SendTextMessageAsync(
                message.Chat.Id,
                "Subscribe:",
                replyMarkup: keyboard);
        }

        private async Task ProcessIncommingPrivateMessage(Message message)
        {
            if (message.Text.StartsWith("/start"))
            {
                var tagId = Int32.Parse(message.Text.Split(' ')[1]);
                await SubscriptionRepository.Add(new Subscription()
                {
                    UserId = message.From.Id,
                    TagId = tagId
                });
            }
            var userSubscriptions = await SubscriptionRepository.FindAll(s => s.UserId.Equals(message.From.Id));
            if (userSubscriptions.Any())
            {
                var keyboard = InlineKeyboardBuilder.BuildPrivateChatKeyboard(userSubscriptions);
                await _client.SendTextMessageAsync(message.Chat.Id, "Your subscriptions:", replyMarkup: keyboard);
            }
        }

        private void NotifyUsers(int[] userIds, string message)
        {
            foreach (var userId in userIds)
            {
                _client.SendTextMessageAsync(userId, "");
            }
        }
    }
}