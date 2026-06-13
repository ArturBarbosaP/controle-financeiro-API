using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MoneyAPI.Data;
using MoneyAPI.Jobs;
using Quartz;
using SwaggerThemes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

//AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

//Dependency Injection Scrutor
builder.Services.Scan(scan =>
{
    scan.FromAssemblyOf<Program>()
    .AddClasses(c => c.InNamespaces("MoneyAPI.Repositories"))
        .AsImplementedInterfaces()
        .WithScopedLifetime()
    .AddClasses(c => c.InNamespaces("MoneyAPI.Services"))
        .AsImplementedInterfaces()
        .WithScopedLifetime();
});

builder.Services.AddSingleton<Session>();
builder.Services.AddSingleton<Notification>();

//BD Entity Framework
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("Default"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Default")),
        assembly => assembly.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName));
});

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClients", policy =>
    {
        policy
            .WithOrigins(builder.Configuration
                .GetSection("AllowedOrigins")
                .Get<string[]>() ?? [])
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Quartz
builder.Services.AddQuartz(q =>
{
    JobKey jobKey = new("AlterarPreLancamentoAndFaturaJob");

    q.AddJob<AlterarPreLancamentoAndFaturaJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opt =>
        opt.ForJob(jobKey)
        .WithIdentity("AlterarPreLancamentoAndFaturaJob-trigger")
        .WithCronSchedule("0 0 8 * * ?") //todod dia as 8 horas
    );
});

builder.Services.AddQuartzHostedService(opt => opt.WaitForJobsToComplete = true);

//Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Money API",
        Version = "v1",
        Description = "API do Money"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Informe o token no formato: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(Theme.UniversalDark, null, c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Money API v1");
    c.RoutePrefix = string.Empty;
});

app.UseCors("AllowClients");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
