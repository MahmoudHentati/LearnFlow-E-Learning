namespace E_learning.Models.DTOs
{
    public class ModuleDTO
    {
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public int Ordre { get; set; }
        public int FormationId { get; set; }
        public FormationDTO? Formation { get; set; }
        public ICollection<VideoDTO> Videos { get; set; } = [];
        public ICollection<DocumentDTO> Documents { get; set; } = [];
        public ICollection<QuizDTO> Quizzes { get; set; } = [];
        public ICollection<TestDTO> Tests { get; set; } = [];
    }
}
