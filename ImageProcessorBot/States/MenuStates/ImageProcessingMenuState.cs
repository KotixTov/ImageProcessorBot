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
    public class ImageProcessingMenuState : CommandMenuState
    {
        public ImageProcessingMenuState(ChatStateMachine stateMachine, ChatId chatId, ITelegramBotClient botClient, CancellationToken cancellationToken, List<IChatCommand> commands) 
            : base(stateMachine, chatId, botClient, cancellationToken, commands)
        {
        }

        public override async Task EnterAsync()
        {
            ReplyKeyboardMarkup keyboard = BuildKeyboard(2);

            await _botClient.SendTextMessageAsync(_chatId, "Choose processing type", replyMarkup: keyboard);
        }

        public override Task ExitAsync()
        {
            return Task.CompletedTask;
        }
    }
}
