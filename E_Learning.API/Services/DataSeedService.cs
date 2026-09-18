using E_learning.Models;
using E_learning.Persistence;
using E_Learning.Domain;
using E_Learning.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.API.Services;

public static class DataSeedService
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);

        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        var subCategories = await dbContext.SousCategories
            .AsNoTracking()
            .OrderBy(sc => sc.Id)
            .ToListAsync();

        if (subCategories.Count == 0)
        {
            return;
        }

        var instructors = await userManager.GetUsersInRoleAsync(AppRoles.Instructor);
        if (instructors.Count == 0)
        {
            return;
        }

        var formations = new List<Formation>();
        var formationSeeds = new (string Title, string Description, string Difficulty, string SubCategoryName)[]
        {
            ("Automatique et contrôle des systèmes", "Maîtrisez les bases des automates, capteurs et boucles de régulation.", "Intermédiaire", "Automatique & contrôle des systèmes"),
            ("Électronique embarquée", "Conception de circuits et introduction aux systèmes embarqués.", "Débutant", "Électronique (circuits, systèmes embarqués)"),
            ("Électrotechnique appliquée", "Machines électriques, conversion d'énergie et dimensionnement.", "Avancé", "Électrotechnique (machines électriques, énergie)"),
            ("Gestion de production", "Planification, ordonnancement et pilotage des flux industriels.", "Intermédiaire", "Gestion de production"),
            ("IA & Machine Learning", "Découvrir les modèles supervisés et non supervisés avec des cas concrets.", "Intermédiaire", "Intelligence artificielle & Machine Learning"),
            ("Réseaux et cybersécurité", "Architecture réseau, sécurité des infrastructures et bonnes pratiques.", "Débutant", "Réseaux & cybersécurité")
        };

        var existingTitles = await dbContext.Formations
            .AsNoTracking()
            .Select(f => f.Titre)
            .ToListAsync();

        for (var i = 0; i < formationSeeds.Length; i++)
        {
            var seed = formationSeeds[i];
            var subCategory = subCategories.FirstOrDefault(sc =>
                string.Equals(sc.Nom, seed.SubCategoryName, StringComparison.OrdinalIgnoreCase))
                ?? subCategories.ElementAtOrDefault(i % subCategories.Count);

            if (subCategory is null)
            {
                continue;
            }

            if (existingTitles.Any(title => string.Equals(title, seed.Title, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            var instructor = instructors[i % instructors.Count];
            formations.Add(new Formation
            {
                Titre = seed.Title,
                Description = seed.Description,
                Prerequis = "Aucun prérequis",
                Difficulte = seed.Difficulty,
                DateCreation = DateTime.UtcNow.AddDays(-i),
                Statut = "Publiée",
                FormateurId = instructor.Id,
                SousCategorieId = subCategory.Id
            });
        }

        dbContext.Formations.AddRange(formations);
        await dbContext.SaveChangesAsync();
    }
}
