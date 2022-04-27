using ImageProcessorBot.Commands;
using ImageProcessorBot.States;
using ImageProcessorBot.States.ImageProcessingStates;
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

            var mainMenuCommands = new List<IChatCommand>();
            var imageProcessingStateCommands = new List<IChatCommand>();

            imageProcessingStateCommands.Add(new ChangeStateCommand(this, new DitherState(this, ChatId, _botClient, _cancellationToken), "/dither"));
            imageProcessingStateCommands.Add(new ChangeStateCommand(this, new PixelateState(this, ChatId, _botClient, _cancellationToken), "/pixelate"));
            imageProcessingStateCommands.Add(new ChangeStateCommand(this, new OilPaintState(this, ChatId, _botClient, _cancellationToken), "/oilpaint"));
            imageProcessingStateCommands.Add(new ChangeStateCommand(this, new SharpenState(this, ChatId, _botClient, _cancellationToken), "/sharpen"));
            imageProcessingStateCommands.Add(new ChangeStateCommand(this, new KodachromeState(this, ChatId, _botClient, _cancellationToken), "/kodachrome"));
            imageProcessingStateCommands.Add(new ChangeStateCommand(this, new PolaroidState(this, ChatId, _botClient, _cancellationToken), "/polaroid"));
            imageProcessingStateCommands.Add(new ChangeStateToDefaultCommand(this, "/menu"));

            mainMenuCommands.Add(new ChangeStateCommand(this, new ImageProcessingMenuState(this, ChatId, _botClient, _cancellationToken, imageProcessingStateCommands), "/processimage"));
            mainMenuCommands.Add(new ShowAPODCommand(botClient, chatId));

            _defaultState = new MainMenuState(this, chatId, botClient, cancellationToken, mainMenuCommands);
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
