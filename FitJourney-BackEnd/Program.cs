using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using BusinessLogic.Context;
using FitJourney_BackEnd.Interface;
using FitJourney_BackEnd.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Permite que o Swagger inclua um botão de autorização para que consigamos inserir o token JWT e possamos testar as rotas protegidas.
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.", Name = "Authorization", In = ParameterLocation.Header, Type = SecuritySchemeType.ApiKey, Scheme = "Bearer" });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// Add default connection string for the Web API controllers
builder.Services.AddDbContext<FitJourneyDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection"))
);

//Permite que a aplicação valide e declare solicitações que contêm um token JWT válido.
//Serão verificados o emissor, a audiência(alvo),a assinatura(autenticidade) e outras informações do token para garantir que ele é válido e confiável.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, ValidateAudience = true, ValidateLifetime = true, ValidateIssuerSigningKey = true, ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
    };
});

//dependencias/Serviços
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISportsRepository, SportsRepository>();
builder.Services.AddScoped<IExercisesRepository, ExercisesRepository>();
builder.Services.AddScoped<IActivitiesRepository, ActivitiesRepository>();
builder.Services.AddScoped<IActivityExerciseRepository, ActivityExerciseRepository>();
builder.Services.AddScoped<IChallengesRepository, ChallengesRepository>();
builder.Services.AddScoped<IChallengeParticipantRepository, ChallengeParticipantRepository>();
builder.Services.AddScoped<IGoalsRepository, GoalsRepository>();

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials());


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();