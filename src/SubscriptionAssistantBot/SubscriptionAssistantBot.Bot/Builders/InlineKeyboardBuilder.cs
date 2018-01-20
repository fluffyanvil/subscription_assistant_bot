using System.Collections.Generic;
using System.Linq;
using SubscriptionAssistantBot.Db.Model;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace SubscriptionAssistantBot.Bot.Builders
{
    public class InlineKeyboardBuilder
    {
        private User _botObject;
        public InlineKeyboardBuilder(User bot)
        {
            _botObject = bot;
        }
        public IReplyMarkup BuildGroupChatKeyboard(IEnumerable<Tag> tags, long chatId)
        {
            var hashtagButtons = tags.Select(tag => new[] { InlineKeyboardButton.WithUrl(tag.Value, $"https://telegram.me/{_botObject.Username}?start={tag.Id}") }).ToArray();
            return new InlineKeyboardMarkup(hashtagButtons);
        }

        public IReplyMarkup BuildPrivateChatKeyboard(IEnumerable<Subscription> subscriptions)
        {
            var subscriptionsButtons = subscriptions.Select(s => new[] { InlineKeyboardButton.WithCallbackData($"{s.Tag.Value} 🗑️", s.Id.ToString()) }).ToArray();
            return new InlineKeyboardMarkup(subscriptionsButtons);
        }
    }
}