namespace E_learning.Models.DTOs
{
    public class SousCategorieDTO
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategorieId { get; set; }
        public CategorieDTO? Categorie { get; set; }
        public ICollection<FormationDTO> Formations { get; set; } = [];
    }
}

