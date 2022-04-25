using ImageProcessorBot.Commands;
using ImageProcessorBot.States;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ImageProcessorBot
{
    public class ChatStateMachine
    {
        public ChatId ChatId { get; private set;}

        private ChatState _defaultState;
        private ChatState _currentState;
        private ITelegramBotClient _botClient;
        private CancellationToken _cancellationToken;

        public ChatStateMachine(ChatId chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            ChatId = chatId;
            _botClient = botClient;
            _cancellationToken = cancellationToken;

            var menuCommands = new List<IChatCommand>();

            menuCommands.Add(new ChangeStateCommand(this, new ImageProcessingState(this, ChatId, _botClient, _cancellationToken)));
            menuCommands.Add(new ShowAPODCommand(botClient, chatId));

            _defaultState = new MenuState(this, chatId, botClient, cancellationToken, menuCommands);
        }

        public async Task HandleUpdateAsync(Update update)
        {
            if (_currentState == null)
            {
                _currentState = _defaultState;
                await _currentState.EnterAsync();
            }

            await _currentState.HandleUpdateAsync(update);
        }

        public void ChangeState(ChatState nextState)
        {
            if (_currentState != null)
                _currentState.ExitAsync();

            _currentState = nextState;

            if (_currentState != null)
                _currentState.EnterAsync();
        }

        public void ChangeStateToDefault()
        {
            ChangeState(_defaultState);
        }
    }
}
