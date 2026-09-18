namespace E_learning.Models.DTOs
{
    public class QuizDTO
    {
        public int Id { get; set; }
        public int DLimite { get; set; }
        public int ScoreMinimum { get; set; }
        public ICollection<QuestionDTO> Questions { get; set; } = [];
        public int ModuleId { get; set; }
        public ModuleDTO? Module { get; set; }
    }
}
