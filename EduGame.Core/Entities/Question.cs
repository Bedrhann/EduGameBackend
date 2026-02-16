namespace EduGame.Core.Entities
{
    public class Question : BaseEntity
    {
        public string Text { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public string WrongAnswer1 { get; set; } = string.Empty;
        public string WrongAnswer2 { get; set; } = string.Empty;
        public string WrongAnswer3 { get; set; } = string.Empty;
        public int DifficultyLevel { get; set; } = 1;

        public int GameContentId { get; set; }
        
        // DÜZELTME:
        public GameContent GameContent { get; set; } = null!;
    }
}