using E_learning.Persistence;
using E_Learning.Domain;
using E_Learning.API.Services;
using E_Learning.API.Mappings;
using E_learning.Interfaces;
using E_learning.Repositories;
using E_learning.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

const string BlazorClientCorsPolicy = "BlazorClientCorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(BlazorClientCorsPolicy, policy =>
    {
        policy.WithOrigins(
                "https://localhost:7168",
                "http://localhost:5237")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IFormationRepository, FormationRepository>();
builder.Services.AddScoped<ICategorieRepository, CategorieRepository>();
builder.Services.AddScoped<ISousCategorieRepository, SousCategorieRepository>();
builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<IVideoRepository, VideoRepository>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IOptionReponseRepository, OptionReponseRepository>();
builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<IRenduTestRepository, RenduTestRepository>();
builder.Services.AddScoped<IParticipationQuizRepository, ParticipationQuizRepository>();
builder.Services.AddScoped<IInscriptionRepository, InscriptionRepository>();
builder.Services.AddScoped<IAvisRepository, AvisRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services
    .AddIdentityCore<AppUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

builder.Services
    .AddAuthentication(IdentityConstants.BearerScheme)
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName?.Replace("+", ".") ?? type.Name);
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
}

await AuthSeedService.SeedAsync(app.Services, builder.Configuration);
await DataSeedService.SeedAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors(BlazorClientCorsPolicy);

app.UseAuthentication();
app.Use(async (context, next) =>
{
    var requestPath = context.Request.Path.Value ?? string.Empty;

    if (context.User.Identity?.IsAuthenticated == true
        && context.User.IsInRole(AppRoles.Instructor)
        && requestPath.StartsWith("/api", StringComparison.OrdinalIgnoreCase)
        && !requestPath.StartsWith("/api/Auth/profile", StringComparison.OrdinalIgnoreCase))
    {
        var userManager = context.RequestServices.GetRequiredService<UserManager<AppUser>>();
        var user = await userManager.GetUserAsync(context.User);

        if (user is not null && !user.IsActive)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Votre compte formateur est en attente de validation par un administrateur.");
            return;
        }
    }

    await next();
});
app.UseAuthorization();

app.MapIdentityApi<AppUser>();
app.MapControllers();

app.Run();
