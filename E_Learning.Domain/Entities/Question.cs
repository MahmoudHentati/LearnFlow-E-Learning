using E_Learning.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Domain.Entities
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public string Enonce { get; set; } = string.Empty;
        public TypeQuestion Type { get; set; }
        public float Bareme { get; set; }
        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }
        public List<OptionReponse> OptionsReponses { get; set; } = [];
    }
}
