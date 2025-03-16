using Users;
using Vaccinations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<RabbitMqPublisher>();
// Add services to the container.
builder.Services.AddGrpcClient<VaccinationService.VaccinationServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:5193");
});

builder.Services.AddGrpcClient<UserService.UserServiceClient>(o =>
{
	o.Address = new Uri("http://localhost:5031");
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ensure RabbitMQ is initialized at startup
using (var scope = app.Services.CreateScope())
{
    var publisher = scope.ServiceProvider.GetRequiredService<RabbitMqPublisher>();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
