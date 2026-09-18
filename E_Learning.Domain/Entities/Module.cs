using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace E_Learning.Domain.Entities
{
    public class Module
    {
        [Key]
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public int Ordre { get; set; }
        public int FormationId { get; set; }
        public Formation? Formation { get; set; }
        public List<Video> Videos { get; set; } = [];
        public List<Quiz> Quizzes { get; set; } = [];
        public List<Test> Tests { get; set; } = [];
        public List<Document> Documents { get; set; } = [];

    }
}
