namespace E_learning.Models.DTOs
{
    public class OptionResponseDTO
    {
        public int Id { get; set; }
        public string Texte { get; set; } = string.Empty;
        public bool EstCorrecte { get; set; }
        public int QuestionId { get; set; }
        public QuestionDTO? Question { get; set; }
    }
}
