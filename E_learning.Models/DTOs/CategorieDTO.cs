namespace E_learning.Models.DTOs
{
    public class CategorieDTO
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<SousCategorieDTO> SousCategories { get; set; } = [];
    }
}
