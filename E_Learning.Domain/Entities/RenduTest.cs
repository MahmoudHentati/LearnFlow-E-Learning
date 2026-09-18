using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Domain.Entities
{
    public class RenduTest
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateSoumission { get; set; }
        public string Contenu { get; set; } = string.Empty;
        public string FichierUrl { get; set; } = string.Empty;
        public float Note { get; set; }
        public string CommentaireFormateur { get; set; } = string.Empty;
        public int TestId { get; set; }
        public Test? Test { get; set; }
        public int InscriptionId { get; set; }
        public Inscription? Inscription { get; set; }
    }
}

