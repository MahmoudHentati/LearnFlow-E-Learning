namespace E_learning.Models.DTOs
{
    public class AvisDTO
    {
        public int Id { get; set; }
        public int Note { get; set; }
        public string Commentaire { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public int InscriptionId { get; set; }
        public InscriptionDTO? Inscription { get; set; }
    }
}
