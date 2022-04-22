using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ImageProcessorBot.States
{
    public abstract class ChatState
    {
        public abstract void Enter();
        public abstract void Exit();
        public abstract Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
    }
}