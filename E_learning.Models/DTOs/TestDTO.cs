namespace E_learning.Models.DTOs
{
    public class TestDTO
    {
        public int Id { get; set; }
        public string Consigne { get; set; } = string.Empty;
        public string FichierConsigneUrl { get; set; } = string.Empty;
        public int ModuleId { get; set; }
        public ModuleDTO? Module { get; set; }
    }
}
