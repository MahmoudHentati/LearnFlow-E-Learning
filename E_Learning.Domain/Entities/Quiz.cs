using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Domain.Entities
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }
        public int DLimite { get; set; }
        public int ScoreMinimum { get; set; }
        public int ModuleId { get; set; }
        public Module? Module { get; set; }
        public List<Question> Questions { get; set; } = [];
    }
}
