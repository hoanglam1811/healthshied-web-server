using System.Text;
using Authentication;
using HealthShield.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Users;
using Vaccinations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<RabbitMqPublisher>();
// Add services to the container.
builder.Services.AddGrpcClient<VaccinationService.VaccinationServiceClient>(o =>
{
    o.Address = new Uri(builder.Configuration["VaccinationService:Url"] ?? "");
});

builder.Services.AddGrpcClient<UserService.UserServiceClient>(o =>
{
	o.Address = new Uri(builder.Configuration["UserService:Url"] ?? "");
});

builder.Services.AddGrpcClient<AuthenticationService.AuthenticationServiceClient>(o =>
{
	o.Address = new Uri(builder.Configuration["UserService:Url"] ?? "");
});

builder.Services.AddScoped<JwtService>();

//Add Jwt
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Scholarship API", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
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
                    }
                },
                Array.Empty<string>()
            }
        });
    }
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]! 
                )),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddAuthorization();

//Add Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
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
// if (app.Environment.IsDevelopment())
// {
	app.UseSwagger();
	app.UseSwaggerUI();
// }

app.UseHttpsRedirection();
app.UseCors("MyAllowPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
