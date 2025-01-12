using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UserManagementService.DAL.Context;
using UserManagementService.DAL.Repositories;
using UserManagementService.DTOs;
using UserManagementService.Services;
using UserManagementService.TokenHandlers;
using UserManagementService.Validators;

var builder = WebApplication.CreateBuilder(args);

ProblemDetailsExtensions.AddProblemDetails(builder.Services);

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));
});
builder.Services.AddMassTransit(x =>
{

    x.SetKebabCaseEndpointNameFormatter();

    x.UsingRabbitMq((context, cfg) =>
    {

        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
        {
            host.Username(builder.Configuration["RabbitMq:Username"]!);
            host.Password(builder.Configuration["RabbitMq:Password"]!);
        });
        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "issuer",
        ValidAudience = "audience",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"]!))
    };
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IValidator<UserDTO>, UserDTOValidator>();
builder.Services.AddScoped<IValidator<AccountVerificationDTO>, AccountVerificationDTOValidator>();
builder.Services.AddScoped<IValidator<AccountVerificationRequestDTO>, AccountVerificationRequestDTOValidator>();
builder.Services.AddScoped<IValidator<LoginDTO>, LoginDTOValidator>();
builder.Services.AddScoped<IValidator<PasswordResetDTO>, PasswordResetDTOValidator>();
builder.Services.AddScoped<IValidator<PasswordResetRequestDTO>, PasswordResetRequestDTOValidator>();
builder.Services.AddScoped<ITokenHandler, UserManagementService.TokenHandlers.TokenHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSwaggerGen(e =>
{
    e.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Please enter token",
        BearerFormat = "JWT",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    e.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });

});

var app = builder.Build();

app.UseProblemDetails();

app.UseSwagger();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();