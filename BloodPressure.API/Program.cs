using BloodPressureApp.API.Controllers;
using BloodPressureApp.API.Models;
using BloodPressureApp.API.Repositories;
using BloodPressureApp.API.Services;
using BloodPressureApp.Core.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BloodpressureContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<BloodPressureRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<BloodPressureService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddTransient<BloodPressureCreateValidator>();
builder.Services.AddTransient<BloodPressureUpdateValidator>();
builder.Services.AddScoped<BloodPressureApp.API.Validators.CreateUserValidator>();

builder.Services.AddScoped<LoginValidator>();

var jwt = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwt["SecretKey"]!);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    });

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BloodpressureContext>();
    context.Database.EnsureCreated();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();