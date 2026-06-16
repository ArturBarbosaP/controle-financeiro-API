using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MoneyAPI.Data;
using MoneyAPI.Helpers;
using MoneyAPI.Jobs;
using Quartz;
using Serilog;
using SwaggerThemes;

var builder = WebApplication.CreateBuilder(args);

//Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .CreateLogger();

builder.Host.UseSerilog();

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
        .WithCronSchedule("0 0 7 * * ?") //todod dia as 7 horas
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

//adicionando usuarioId e IP no log
app.UseSerilogRequestLogging(opts =>
{
    opts.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        var token = httpContext.Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
        var session = httpContext.RequestServices.GetRequiredService<Session>();
        var usuarioId = token != null ? session.ObterUsuarioId(token) : null;

        diagnosticContext.Set("UsuarioId", usuarioId ?? 0);
        diagnosticContext.Set("IP", httpContext.Connection.RemoteIpAddress);
    };
});

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();

//tema escuro pro swagger
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
