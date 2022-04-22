using ImageProcessorBot.States;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ImageProcessorBot
{
    public class ChatStateMachine
    {
        public User User { get; private set;}

        private ChatState _currentState;

        public ChatStateMachine(User user)
        {
            User = user;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (_currentState == null)
                return;

            await _currentState.HandleUpdateAsync(botClient, update, cancellationToken);
        }

        public void ChangeState(ChatState nextState)
        {
            if (_currentState != null)
                _currentState.Exit();

            _currentState = nextState;

            if (_currentState != null)
                _currentState.Enter();
        }
    }
}
