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
        private List<ChatStateMachine> _chatStateMachines;

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            User user;

            if (TryGetUserFromUpdate(update, out user) == false)
            {
                return;
            }

            foreach(var stateMachine in _chatStateMachines)
            {
                if (stateMachine.User == user)
                {
                    await stateMachine.HandleUpdateAsync(botClient, update, cancellationToken);
                    return;
                }
            }

            _chatStateMachines.Add(new ChatStateMachine(user));
        }

        private bool TryGetUserFromUpdate(Update update, out User user)
        {
            if (update.Type == UpdateType.Message)
            {
                user = update.Message.From;
                return true;
            }
            else if (update.Type == UpdateType.CallbackQuery)
            {
                user = update.CallbackQuery.From;
                return true;
            }

            user = null;
            return false;
        }
    }
}
