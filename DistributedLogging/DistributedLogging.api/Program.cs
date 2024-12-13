using DistributedLogging.api.Auth.Handlers;
using DistributedLogging.Application;
using DistributedLogging.common;
using DistributedLogging.Presistence;
using DistributedLogging.Presistence.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()  // Log to console
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) // Log to files daily
    .WriteTo.File(
        path: "Logs/EnteredLogs-log-.txt", // Secondary 
        rollingInterval: RollingInterval.Day, // Rolling file per day
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information, 
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();
var jwtSettings = SettingsManager.JWTSettings;
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDBContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.ValidIssuer,
        ValidAudience = jwtSettings.ValidAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(jwtSettings.SecurityKey))
    };
});
builder.Services.AddCors(options =>
{
    // this defines a CORS policy called "default"
    options.AddPolicy("Cors", policy =>
    {
        policy.WithOrigins(["http://localhost:4200"])
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddScoped<JwtHandler>();
// Add Serilog to the application
builder.Host.UseSerilog();// Add services to the container.
builder.Services.AddApplicationLayer().AddPresistenceInfrastructure();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("Cors");

app.MapControllers();

app.Run();
