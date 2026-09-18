using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace E_Learning.Domain.Entities
{
    public class Video
    {
        [Key]
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public int Ordre { get; set; }
        public string UrlVideo { get; set; } = string.Empty;
        public int Duree { get; set; }
        public int ModuleId { get; set; }
        public Module? Module { get; set; }
    }
}
