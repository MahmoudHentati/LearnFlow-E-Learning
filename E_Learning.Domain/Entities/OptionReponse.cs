using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Learning.Domain.Entities
{
    public class OptionReponse
    {
        [Key]
        public int Id { get; set; }
        public string Texte { get; set; } = string.Empty;
        public bool EstCorrecte { get; set; }
        public int QuestionId { get; set; }
        public Question? Question { get; set; }
    }
}
