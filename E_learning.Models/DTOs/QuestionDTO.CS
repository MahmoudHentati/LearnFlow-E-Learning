using E_Learning.Models.Enums;

namespace E_learning.Models.DTOs
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Enonce { get; set; } = string.Empty;
        public TypeQuestion Type { get; set; }
        public float Bareme { get; set; }
        public int QuizId { get; set; }
        public QuizDTO? Quiz { get; set; }
        public ICollection<OptionResponseDTO> OptionsReponses { get; set; } = [];
    }
}
