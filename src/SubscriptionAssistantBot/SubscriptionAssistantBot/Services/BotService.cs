using System;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace SubscriptionAssistantBot.Services
{
    public class BotService : IBotService
    {
        private readonly SubscriptionAssistantBotSettings _config;
        public TelegramBotClient Api { get; }

        public BotService(IOptions<SubscriptionAssistantBotSettings> settings)
        {
            _config = settings.Value;
            Api = new TelegramBotClient(_config.Token);
            var getme = Api.GetMeAsync();
            Console.WriteLine(getme.Result.FirstName);
            Api.StartReceiving();
        }
    }
}