using Microsoft.EntityFrameworkCore;
using UserManagementService.DbContexts;
using UserManagementService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton<RabbitMqConsumer>();
builder.Services.AddDbContext<UserDbContext>(options =>
	options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
	  ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")))
);

var app = builder.Build();

var rabbitMqConsumer = app.Services.GetRequiredService<RabbitMqConsumer>();
await Task.Run(async () => await rabbitMqConsumer.StartConsuming());

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<UserServiceImpl>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
