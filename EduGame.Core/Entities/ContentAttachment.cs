namespace EduGame.Core.Entities
{
    public class ContentAttachment : BaseEntity
    {
        public string FileUrl { get; set; } = string.Empty; // Dosyanın sunucudaki yolu
        public string FileType { get; set; } = "application/pdf"; // pdf, png, jpg...

        // Hangi içerik paketine ait?
        public int GameContentId { get; set; }
        public GameContent GameContent { get; set; } = null!;
    }
}