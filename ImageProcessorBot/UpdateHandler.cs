using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ImageProcessorBot
{
    class UpdateHandler
    {
        private List<ChatStateMachine> _chatStateMachines = new List<ChatStateMachine>();

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (TryGetChatIdFromUpdate(update, out ChatId chatId) == false)
            {
                return;
            }

            foreach (var stateMachine in _chatStateMachines)
            {
                //Need to solve the problem with == operator for ChatId
                if (stateMachine.ChatId.Identifier == chatId.Identifier)
                {
                    await stateMachine.HandleUpdateAsync(update);
                    return;
                }
            }

            var newStateMachine = new ChatStateMachine(chatId, botClient, cancellationToken);
            _chatStateMachines.Add(newStateMachine);
            await newStateMachine.HandleUpdateAsync(update);
        }

        private bool TryGetChatIdFromUpdate(Update update, out ChatId user)
        {
            if (update.Type == UpdateType.Message)
            {
                user = update.Message.Chat.Id;
                return true;
            }
            else if (update.Type == UpdateType.CallbackQuery)
            {
                user = update.CallbackQuery.Message.Chat.Id;
                return true;
            }

            user = null;
            return false;
        }
    }
}
