using Microsoft.EntityFrameworkCore;
using Server.Middlewares;
using Server.Persistence;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);
var forceLocalhost = builder.Configuration.GetValue<bool>("ForceLocalhost");

// Para testes na própria máquina com test_agent.py etc.
if (forceLocalhost)
{
    Console.WriteLine("ForceLocalhost: true");
    builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");
}
else
{
    Console.WriteLine("ForceLocalhost: false");
}

// Conexão ao banco de dados
var dbServer = builder.Configuration["SqlServerConnection:Server"];
var dbName = builder.Configuration["SqlServerConnection:Database"];
var dbUser = builder.Configuration["SqlServerConnection:User"];
var dbPassword = Environment.GetEnvironmentVariable("SQL_SERVER_PASS");

if (string.IsNullOrWhiteSpace(dbPassword))
{
    throw new InvalidOperationException("SQL_SERVER_PASS is not defined.");
}

var dbConnectionString =
    $"Data Source={dbServer};" +
    $"Initial Catalog={dbName};" +
    $"User Id={dbUser};" +
    $"Password={dbPassword};" +
    $"TrustServerCertificate=True;";

// DbContext
builder.Services.AddDbContext<ArquivoLanDbContext>(options =>
{
    options.UseSqlServer(dbConnectionString);
});

// Services (DI Registry)
builder.Services.AddControllers();
builder.Services.AddScoped<IFileEntryService, FileEntryService>();
builder.Services.AddScoped<IComputerService, ComputerService>();

var app = builder.Build();

// Verificação de banco
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ArquivoLanDbContext>();
    
    if (!dbContext.Database.CanConnect())
    {
        Console.WriteLine($"Can't connect to database '{dbName}'.");
        Environment.Exit(1);
    }
    
    dbContext.Database.EnsureCreated();
}

// Middlewares (ordem recomendada)
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<MaxRequestBodySizeMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();