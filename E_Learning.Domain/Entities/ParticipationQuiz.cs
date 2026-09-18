using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Domain.Entities
{
    public class ParticipationQuiz
    {
        [Key]
        public int Id { get; set; }
        public DateTime DatePassage { get; set; }
        public float Score { get; set; }
        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }
        public int InscriptionId { get; set; }
        public Inscription? Inscription { get; set; }
    }
}
