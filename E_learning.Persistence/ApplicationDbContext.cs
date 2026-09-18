using E_Learning.Domain;
using E_Learning.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_learning.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {

        }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<SousCategorie> SousCategories { get; set; }
        public DbSet<Formation> Formations { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Inscription> Inscriptions { get; set; }
        public DbSet<Avis> Avis { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<OptionReponse> Reponses { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<RenduTest> RenduTests { get; set; }
        public DbSet<ParticipationQuiz> ParticipationQuizzes { get; set; }
    }
}
