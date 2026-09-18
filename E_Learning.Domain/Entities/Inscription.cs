using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Domain.Entities
{
    public class Inscription
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateInscription { get; set; }
        public float Progression { get; set; }
        public string EdudiantId { get; set; } = string.Empty;
        public AppUser? Edudiant { get; set; }
        public int FormationId { get; set; }
        public Formation? Formation { get; set; }
        public List<Avis> Avis { get; set; } = [];

    }
}
