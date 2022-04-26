using ImageProcessorBot.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ImageProcessorBot.States
{
    public abstract class CommandMenuState : ChatState
    {
        protected List<IChatCommand> _commands = new List<IChatCommand>();

        public CommandMenuState(ChatStateMachine stateMachine, ChatId chatId, ITelegramBotClient botClient, CancellationToken cancellationToken, List<IChatCommand> commands)
            : base(stateMachine, chatId, botClient, cancellationToken)
        {
            _commands = commands;
        }

        public abstract override Task EnterAsync();

        public abstract override Task ExitAsync();

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

        protected ReplyKeyboardMarkup BuildKeyboard(int buttonsInARow)
        {
            var buttonRows = new KeyboardButton[_commands.Count / buttonsInARow + 1][];

            for (int i = 0; i < buttonRows.Length - 1; i++)
            {
                buttonRows[i] = new KeyboardButton[buttonsInARow];
            }

            buttonRows[buttonRows.Length - 1] = new KeyboardButton[_commands.Count % buttonsInARow];

            for (int i = 0; i < _commands.Count; i++)
            {
                buttonRows[i / buttonsInARow][i % buttonsInARow] = new KeyboardButton(_commands[i].CommandText);
            }

            var keyboard = new ReplyKeyboardMarkup(buttonRows)
            {
                ResizeKeyboard = true
            };

            return keyboard;
        }
    }
}
