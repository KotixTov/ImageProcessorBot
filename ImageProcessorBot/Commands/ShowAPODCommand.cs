using System.Threading.Tasks;
using Apod;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ImageProcessorBot.Commands
{
    class ShowAPODCommand : IChatCommand
    {
        private string _commandText = "/AstronomyPictureOfTheDay";
        private ApodClient _apodClient;
        private readonly ITelegramBotClient _botClient;
        private readonly ChatId _chatId;

        string IChatCommand.CommandText { get => _commandText;}

        public ShowAPODCommand(ITelegramBotClient botClient, ChatId chatId)
        {
            _apodClient = new ApodClient();
            _botClient = botClient;
            _chatId = chatId;
        }
        
        async Task ICommand.Execute()
        {
            var response = await _apodClient.FetchApodAsync();
            var content = response.Content;

            if (content.MediaType == MediaType.Image)
            {
                await _botClient.SendPhotoAsync(_chatId, content.ContentUrl, "Picture of the day from NASA");
            }
            if (content.MediaType == MediaType.Video)
            {
                await _botClient.SendVideoAsync(_chatId, content.ContentUrl, caption: "Video of the day from NASA");
            }
        }
    }
}
