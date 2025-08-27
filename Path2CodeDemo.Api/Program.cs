using Path2CodeDemo.Application.IService;
using Path2CodeDemo.Application.Service;
using Path2CodeDemo.Application.IRepository;
using Path2CodeDemo.Infrastructure.Repository;
using Path2CodeDemo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

try
{
    Log.Information("Starting web host");
    var builder = WebApplication.CreateBuilder(args);

    // Configure Serilog
    builder.Host.UseSerilog((context, loggerConfiguration) =>
        loggerConfiguration.WriteTo.Console()
        .ReadFrom.Configuration(context.Configuration));

    // Add DbContext with PostgreSQL
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Add services to the container.
    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    builder.Services.AddSwaggerGen(options =>
    {
        options.IncludeXmlComments(xmlPath);
    });

    // Dependency Injection
    builder.Services.AddScoped<ICandidateService, CandidateService>();
    builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging(); // Logs HTTP requests

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}