using ImageProcessorBot.Commands;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ImageProcessorBot.States
{
    class MenuState : ChatState
    {
        private List<IChatCommand> _commands = new List<IChatCommand>();

        public MenuState(ChatStateMachine stateMachine, ChatId chatId, ITelegramBotClient botClient, CancellationToken cancellationToken, List<IChatCommand> commands) 
            : base (stateMachine, chatId, botClient, cancellationToken)
        {
            _commands = commands;
        }

        public override async Task EnterAsync()
        {
            ReplyKeyboardMarkup keyboard = BuildKeyboard();

            await _botClient.SendTextMessageAsync(_chatId, "This is chat menu", replyMarkup: keyboard);
        }

        private ReplyKeyboardMarkup BuildKeyboard()
        {
            var buttonLines = new KeyboardButton[_commands.Count / 3 + 1][];

            for (int i = 0; i < buttonLines.Length - 1; i++)
            {
                buttonLines[i] = new KeyboardButton[3];
            }

            buttonLines[buttonLines.Length - 1] = new KeyboardButton[_commands.Count % 3];

            for (int i = 0; i < _commands.Count; i++)
            {
                buttonLines[i / 3][i % 3] = new KeyboardButton(_commands[i].CommandText);
            }

            var keyboard = new ReplyKeyboardMarkup(buttonLines)
            {
                ResizeKeyboard = true
            };

            return keyboard;
        }

        public override async Task ExitAsync()
        {
        }

        public override async Task HandleUpdateAsync(Update update)
        {
            if (update.Type != UpdateType.Message)
            {
                return;
            }

            var message = update.Message;

            if (message.Type == MessageType.Text)
            {
                var messageText = message.Text;

                foreach (var comand in _commands)
                {
                    if (messageText == comand.CommandText)
                    {
                        await comand.Execute();
                    }
                }
            }
        }
    }
}
