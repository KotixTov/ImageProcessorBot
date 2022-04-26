﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ImageProcessorBot.States.ImageProcessingStates
{
    class DitherState : ImageProcessingState
    {
        public DitherState(ChatStateMachine stateMachine, ChatId chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
            : base(stateMachine, chatId, botClient, cancellationToken)
        {
        }

        protected override void ProcessImage()
        {
            using (Image image = Image.Load(_imagePath + _savedImageName))
            {
                image.Mutate(x => x.Dither());

                image.SaveAsPng(_imagePath + _processedImageName);
            }
        }
    }
}