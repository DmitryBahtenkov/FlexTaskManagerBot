using System.Reflection.Metadata;
namespace FTM.Domain.Helpers
{
    public enum MediaType
    {
        // одно вложение + подпись
        Photo,
        Video,
        Audio,
        Document
        //несколько вложений - тг посылает несколько update с единым MediaGroupId на каждый экземпляр вложения
        //,MediaGroup
        //невозможно прикрепить текст
        // , Voice, VideoNote, Location, Contact
    }
}