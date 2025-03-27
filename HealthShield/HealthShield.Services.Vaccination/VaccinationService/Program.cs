using Microsoft.EntityFrameworkCore;
using VaccinationService.DbContexts;
using VaccinationService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<RabbitMqConsumer>();
// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); 
builder.Services.AddDbContext<VaccinationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
      ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")))
);
builder.Services.AddAutoMapper(typeof(VaccineProfile)); 
var app = builder.Build();
var rabbitMqConsumer = app.Services.GetRequiredService<RabbitMqConsumer>();
await Task.Run(async () => await rabbitMqConsumer.StartConsuming());

// Configure the HTTP request pipeline.
app.MapGrpcService<VaccinationServiceImpl>();
// app.MapGrpcService<PackageServiceImpl>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
