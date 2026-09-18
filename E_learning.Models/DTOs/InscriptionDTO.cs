namespace E_learning.Models.DTOs
{
    public class InscriptionDTO
    {
        public int Id { get; set; }
        public DateTime? DateInscription { get; set; }
        public float Progression { get; set; }
        public string EdudiantId { get; set; } = string.Empty;
        public AppUserDTO? Edudiant { get; set; }
        public int FormationId { get; set; }
        public FormationDTO? Formation { get; set; }
        public ICollection<AvisDTO> Avis { get; set; } = [];
    }
}
