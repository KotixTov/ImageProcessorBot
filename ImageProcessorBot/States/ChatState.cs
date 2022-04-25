using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ImageProcessorBot.States
{
    public abstract class ChatState
    {
        protected ChatStateMachine _stateMachine;
        protected ITelegramBotClient _botClient;
        protected CancellationToken _cancellationToken;
        protected ChatId _chatId;

        public ChatState(ChatStateMachine stateMachine, ChatId chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            _stateMachine = stateMachine;
            _chatId = chatId;
            _botClient = botClient;
            _cancellationToken = cancellationToken;
        }

        public abstract Task EnterAsync();

        public abstract Task ExitAsync();
        public abstract Task HandleUpdateAsync(Update update);
    }
}