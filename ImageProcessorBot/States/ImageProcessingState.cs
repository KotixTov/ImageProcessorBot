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
    public class ImageProcessingState : ChatState
    {
        public ImageProcessingState(ChatStateMachine stateMachine, ChatId chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
            : base(stateMachine, chatId, botClient, cancellationToken)
        {

        }

        public override async Task EnterAsync()
        {
            ReplyKeyboardMarkup keyboard = new ReplyKeyboardMarkup(new KeyboardButton[][]{
                new KeyboardButton[]{"Dithering", "Black&White"},
                new KeyboardButton[]{"OilPainting", "Pixelate"},
                new KeyboardButton[]{"/menu"}
            });

            await _botClient.SendTextMessageAsync(_chatId, "Choose processing type", replyMarkup: keyboard);
        }

        public override async Task ExitAsync()
        {
            await _botClient.SendTextMessageAsync(_chatId, "", replyMarkup: new ReplyKeyboardRemove());
        }

        public async override Task HandleUpdateAsync(Update update)
        {
            if (update.Type != UpdateType.Message)
            {
                return;
            }

            var messageText = update.Message.Text;

            switch (messageText)
            {
                case "Dithering":
                case "Black&White":
                case "OilPainting":
                case "Pixelate":
                    await _botClient.SendTextMessageAsync(_chatId, $"You've choosed {messageText}");
                    break;
                case "/menu":
                    _stateMachine.ChangeStateToDefault();
                    break;
                default:
                    await _botClient.SendTextMessageAsync(_chatId, "Choose right command");
                    break;
            }

        }
    }
}
