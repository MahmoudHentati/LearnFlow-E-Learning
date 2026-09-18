namespace E_learning.Models.DTOs
{
    public class FormationDTO
    {
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Prerequis { get; set; } = string.Empty;
        public string Difficulte { get; set; } = string.Empty;
        public DateTime? DateCreation { get; set; }
        public string Statut { get; set; } = string.Empty;
        public string FormateurId { get; set; } = string.Empty;
        public string FormateurNom { get; set; } = string.Empty;
        public int SousCategorieId { get; set; }
        public SousCategorieDTO? SousCategorie { get; set; }
        public ICollection<ModuleDTO> Modules { get; set; } = [];
    }
}
