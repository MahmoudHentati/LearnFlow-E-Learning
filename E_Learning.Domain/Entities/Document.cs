using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace E_Learning.Domain.Entities
{
    public class Document
    {
        [Key]
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public int Ordre { get; set; }
        public string UrlFichier { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public int ModuleId { get; set; }
        public Module? Module { get; set; }

    }

}
