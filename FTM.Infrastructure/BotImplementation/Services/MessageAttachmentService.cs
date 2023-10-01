using FTM.Domain.Helpers;
using FTM.Domain.Models.IssueFileModel;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.BotImplementation.Services
{
    public class MessageAttachmentService
    {
        public IssueFile GetAttached(Message message)
        {
            MediaType attachmentType = MediaType.Document;
            FileBase file = message.Document!;

            //tbd посмотреть более оптимальный способ

            if (message.Photo is not null)
            {
                attachmentType = MediaType.Photo;
                file = message.Photo.Last();
            }

            if (message.Audio is not null)
            {
                attachmentType = MediaType.Audio;
                file = message.Audio;
            }

            if (message.Video is not null)
            {
                attachmentType = MediaType.Video;
                file = message.Video;
            }

            var attachment = GetAttachmentInfo(file);
            attachment.Type = attachmentType;

            return attachment;
        }

        private IssueFile GetAttachmentInfo(FileBase file) => new()
        {
            FileId = file.FileId
        };
    }
}