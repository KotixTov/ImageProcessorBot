using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace ImageProcessorBot.States
{
    public abstract class ImageProcessingState : ChatState
    {
        protected string _imagePath = @"TempImages\";
        protected string _savedImageName = "savedImage.jpg";
        protected string _processedImageName = "processedImage.png";

        public ImageProcessingState(ChatStateMachine stateMachine, ChatId chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
            : base(stateMachine, chatId, botClient, cancellationToken)
        {
        }

        public async override Task EnterAsync()
        {
            await _botClient.SendTextMessageAsync(_chatId, "Send image to process", replyMarkup: new ReplyKeyboardRemove());
        }

        public override Task ExitAsync()
        {
            return Task.CompletedTask;
        }

        public async override Task HandleUpdateAsync(Update update)
        {
            if (update.Type != UpdateType.Message)
            {
                return;
            }

            var message = update.Message;

            if (TryGetImageFileId(message, out var fileId))
            {
                var file = await _botClient.GetFileAsync(fileId, _cancellationToken);
                await HandleFile(file);
                _stateMachine.ChangeStateToDefault();
            }
            else if (message.Type == MessageType.Text)
            {
                if (message.Text == "/menu")
                {
                    _stateMachine.ChangeStateToDefault();
                }
            }
            else
            {
                await _botClient.SendTextMessageAsync(_chatId, "Send correct image or image document");
            }
        }

        private bool TryGetImageFileId(Message message, out string fileId)
        {
            if (message.Type == MessageType.Photo)
            {
                fileId = message.Photo[message.Photo.Length - 1].FileId;
                return true;
            }
            else if (message.Type == MessageType.Document)
            {
                var document = message.Document;
                if (document.MimeType.Contains("image"))
                {
                    fileId = message.Document.FileId;
                    return true;
                }
            }

            fileId = null;
            return false;
        }

        private async Task HandleFile(Telegram.Bot.Types.File file)
        {
            await DownloadFile(file);

            ProcessImage();

            await SendImageFileAsync();

            return;
        }

        protected abstract void ProcessImage();

        private async Task DownloadFile(Telegram.Bot.Types.File file)
        {
            using (var saveImageStream = new FileStream(_imagePath + _savedImageName, FileMode.Create))
            {
                await _botClient.DownloadFileAsync(file.FilePath, saveImageStream, _cancellationToken);
            }
        }

        private async Task SendImageFileAsync()
        {
            using (var sendImageStream = System.IO.File.Open(_imagePath + _processedImageName, FileMode.Open))
            {
                var inputMediaPhoto = new InputOnlineFile(sendImageStream, "ProcessedImage.png");
                await _botClient.SendDocumentAsync(_chatId,
                    document: inputMediaPhoto,
                    caption: "Your processed image",
                    parseMode: ParseMode.Markdown,
                    cancellationToken: _cancellationToken);
            }
        }
    }
}
