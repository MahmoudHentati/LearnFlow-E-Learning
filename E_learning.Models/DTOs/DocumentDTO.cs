namespace E_learning.Models.DTOs
{
    public class DocumentDTO
    {
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public int Ordre { get; set; }
        public string UrlFichier { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public int ModuleId { get; set; }
        public ModuleDTO? Module { get; set; }
    }
}
