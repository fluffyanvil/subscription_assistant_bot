using Microsoft.Extensions.Logging;

namespace SubscriptionAssistantBot.Services
{
    public class UpdateService : IUpdateService
    {
        public UpdateService(IBotService botService)
        {
            BotService = botService;
        }

        public IBotService BotService { get; }
    }
}