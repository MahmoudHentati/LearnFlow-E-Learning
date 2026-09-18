using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Domain.Entities
{
    public class Avis
    {
        public int Id { get; set; }
        public int Note { get; set; }
        public string Commentaire { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int InscriptionId { get; set; }
        public Inscription? Inscription { get; set; }
    }
}
