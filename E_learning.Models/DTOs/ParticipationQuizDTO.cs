namespace E_learning.Models.DTOs
{
    public class ParticipationQuizDTO
    {
        public int Id { get; set; }
        public DateTime? DatePassage { get; set; }
        public float Score { get; set; }
        public int QuizId { get; set; }
        public QuizDTO? Quiz { get; set; }
        public int InscriptionId { get; set; }
        public InscriptionDTO? Inscription { get; set; }
    }
}
