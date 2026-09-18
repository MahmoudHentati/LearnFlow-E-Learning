namespace E_learning.Models.DTOs
{
    public class RenduTestDTO
    {
        public int Id { get; set; }
        public DateTime? DateSoumission { get; set; }
        public string Contenu { get; set; } = string.Empty;
        public string FichierUrl { get; set; } = string.Empty;
        public float? Note { get; set; }
        public string CommentaireFormateur { get; set; } = string.Empty;
        public int TestId { get; set; }
        public TestDTO? Test { get; set; }
        public int InscriptionId { get; set; }
        public InscriptionDTO? Inscription { get; set; }
    }
}
