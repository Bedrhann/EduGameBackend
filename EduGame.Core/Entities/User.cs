using System.Collections.Generic;

namespace EduGame.Core.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        // DÜZELTME: Listeyi başlattık. Artık null olamaz, boş liste olur.
        public ICollection<GameContent> Contents { get; set; } = new List<GameContent>();
    }
}