using Telegram.Bot;

namespace SubscriptionAssistantBot.Services
{
    public interface IBotService
    {
        TelegramBotClient Api { get; }
    }
}