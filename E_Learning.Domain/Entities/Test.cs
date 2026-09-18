using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Domain.Entities
{
    public class Test
    {
        [Key]
        public int Id { get; set; }
        public string Consigne { get; set; } = string.Empty;
        public string FichierConsigneUrl { get; set; } = string.Empty;
        public int ModuleId { get; set; }
        public Module? Module { get; set; }
    }
}
