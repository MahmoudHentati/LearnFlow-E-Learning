namespace E_learning.Models.DTOs
{
    public class VideoDTO
    {
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public int Ordre { get; set; }
        public string UrlVideo { get; set; } = string.Empty;
        public int Duree { get; set; }
        public int ModuleId { get; set; }
        public ModuleDTO? Module { get; set; }
    }
}
