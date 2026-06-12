using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Server.Dtos.Request;
using Server.Middlewares;
using Server.Persistence;
using Server.Services;
using Server.Services.Interfaces;

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
builder.Services.AddScoped<IComputerService, ComputerService>();
builder.Services.AddScoped<ISnapshotService, SnapshotService>();
builder.Services.AddScoped<IDirEntryService, DirEntryService>();
builder.Services.AddScoped<IFileEntryService, FileEntryService>();

var app = builder.Build();

// Ambiente dev
if (app.Environment.IsDevelopment())
{
    Console.WriteLine("IsDevelopment: true");
    Console.WriteLine();
    Console.WriteLine("___________________________________________");
    Console.WriteLine("[Exemplos de payloads]");
    Console.WriteLine();
    // Printar no terminal alguns exemplos de payloads
    // que são aceitos em endpoints do sistema
    
    // ComputerRequestDto payload
    var exComputerRequestDto = new ComputerRequestDto
    {
        Hostname = "Unknown",
        Fqdn = "Unknown",
        DomainName = "Unknown",
        Ipv4Address = "192.168.0.10",
        MacAddress = "00:11:22:33:44:55",
        OperatingSystem = "Windows",
        AgentId = Guid.NewGuid().ToString(),
        AgentVersion = "1.0.0"
    };
    Console.WriteLine("[ComputerRequestDto]");
    Console.WriteLine(JsonSerializer.Serialize(exComputerRequestDto));
    
    // DirEntryRequestDto payload
    var exDirEntryRequestDto = new DirEntryRequestDto
    {
        Path = @"C:\Users\Teste\Documents",
        FileCount = 150,
        SubdirectoryCount = 12,
        SizeBytes = 52428800,
        CreatedAtUtc = DateTime.UtcNow.AddMonths(-6),
        LastSeenAtUtc = DateTime.UtcNow
    };
    Console.WriteLine("[DirEntryRequestDto]");
    Console.WriteLine(JsonSerializer.Serialize(exDirEntryRequestDto));
    
    // FileEntryRequestDto payload
    var exFileEntryRequestDto = new FileEntryRequestDto
    {
        Path = @"C:\Teste\Documents\documento.txt",
        Name = "documento.txt",
        Extension = ".txt",
        SizeBytes = 2048,
        Sha256 = "a3f5c7d9e1b2a4c6d8f0a1b3c5d7e9f1a2b4c6d8e0f1a3b5c7d9e1f2a4b6c8d0",
        LastWriteTimeUtc = DateTime.UtcNow.AddHours(-2),
        LastSeenUtc = DateTime.UtcNow,
        CreatedTimeUtc = DateTime.UtcNow.AddDays(-30),
        LastAccessTimeUtc = DateTime.UtcNow.AddMinutes(-10),
        IsReadOnly = false,
        Attributes = "Archive"
    };
    Console.WriteLine("[FileEntryRequestDto]");
    Console.WriteLine(JsonSerializer.Serialize(exFileEntryRequestDto));
    
    // SnapshotRequestDto payload
    var exSnapshotRequestDto = new SnapshotRequestDto
    {
        StartedAtUtc = DateTime.UtcNow.AddMinutes(-2),
        FinishedAtUtc = DateTime.UtcNow,
        TargetPath = @"C:\Teste\Documents"
    };
    Console.WriteLine("[SnapshotRequestDto]");
    Console.WriteLine(JsonSerializer.Serialize(exSnapshotRequestDto));

    // GatheringRequestDto payload
    List<FileEntryRequestDto> fileEntryRequestDtoList = new List<FileEntryRequestDto>();
    fileEntryRequestDtoList.Add(exFileEntryRequestDto);

    var exGatheringRequestDto = new GatheringRequestDto
    {
        Computer = exComputerRequestDto,
        DirEntry = exDirEntryRequestDto,
        FileEntries = fileEntryRequestDtoList,
        Snapshot = exSnapshotRequestDto
    };
    Console.WriteLine("[GatheringRequestDto]");
    Console.WriteLine(JsonSerializer.Serialize(exGatheringRequestDto));
    Console.WriteLine();
    Console.WriteLine("___________________________________________");
}

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