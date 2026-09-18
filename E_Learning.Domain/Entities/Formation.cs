using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace E_Learning.Domain.Entities
{
    public class Formation
    {
        [Key]
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Prerequis { get; set; } = string.Empty;
        public string Difficulte { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; }
        public string Statut { get; set; } = string.Empty;
        public string FormateurId { get; set; } = string.Empty;
        public AppUser? Formateur { get; set; }
        public int SousCategorieId { get; set; }
        public SousCategorie? SousCategorie { get; set; }
        public List<Module> Modules { get; set; } = [];

    }
}
