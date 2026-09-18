using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace E_Learning.Domain.Entities
{
    public class SousCategorie
    {
        [Key]
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategorieId { get; set; }
        public Categorie? Categorie { get; set; }
        public List<Formation> Formations { get; set; } = [];
    }
}
