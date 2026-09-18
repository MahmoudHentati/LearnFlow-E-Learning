using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Domain.Entities
{
    public class Categorie
    {
        [Key]
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<SousCategorie> SousCategories { get; set; } = [];
    }
}
