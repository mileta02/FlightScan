using FlightScan.Application.Behaviours;
using FlightScan.Application.Cqrs.Commands.Auth;
using FlightScan.Application.Handlers.Auth;
using FlightScan.Api.Hubs;
using FlightScan.Api.Middlewares;
using FlightScan.Api.Services;
using FlightScan.Core.Config;
using FlightScan.Core.Entities;
using FlightScan.Core.Enums;
using FlightScan.Core.Interfaces;
using FlightScan.Infrastructure.Data;
using FlightScan.Infrastructure.Data.Context;
using FlightScan.Infrastructure.Data.Repository;
using FlightScan.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using FlightScan.Application.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();

// JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<IJwtService, JwtService>();

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
            ValidIssuer = jwtSettings.Issuer,
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                var token = ctx.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(token) &&
                    ctx.HttpContext.Request.Path.StartsWithSegments("/hubs/flight"))
                    ctx.Token = token;
                return Task.CompletedTask;
            }
        };
    });

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginHandler).Assembly));

// FluentValidation Pipeline Behavior
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssembly(typeof(LoginCommand).Assembly);

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

// UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// SignalR
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();
builder.Services.AddScoped<IReservationNotificationService, ReservationNotificationService>();

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(MappingProfile).Assembly);
});

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    if (!await db.Users.AnyAsync(u => u.Role == UserRole.Administrator))
    {
        var adminUsername = app.Configuration["Seed:AdminUsername"];
        var adminPassword = app.Configuration["Seed:AdminPassword"];

        db.Users.Add(new User
        {
            Username = adminUsername,
            Password = BCrypt.Net.BCrypt.HashPassword(adminPassword),
            Role = UserRole.Administrator
        });

        await db.SaveChangesAsync();
    }
}

// Middlewares
app.UseMiddleware<ErrorMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowFrontend");

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<FlightHub>("/hubs/flight");

app.Run();
