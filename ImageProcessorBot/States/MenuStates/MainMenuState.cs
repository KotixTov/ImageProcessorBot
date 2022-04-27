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
    class MainMenuState : CommandMenuState
    {
        public MainMenuState(ChatStateMachine stateMachine, ChatId chatId, ITelegramBotClient botClient, CancellationToken cancellationToken, List<IChatCommand> commands) 
            : base (stateMachine, chatId, botClient, cancellationToken, commands) { }

        public override async Task EnterAsync()
        {
            ReplyKeyboardMarkup keyboard = BuildKeyboard(3);

            await _botClient.SendTextMessageAsync(_chatId, "Chose next command. There aren't many at the moment, but there will be updates in the future!", replyMarkup: keyboard);
        }

        public override Task ExitAsync()
        {
            return Task.CompletedTask;
        }
    }
}
