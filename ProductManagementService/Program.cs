using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductManagementService.Consumers;
using ProductManagementService.DAL.Context;
using ProductManagementService.DAL.Repositories;
using ProductManagementService.DTOs;
using ProductManagementService.Services;
using ProductManagementService.Validators;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

ProblemDetailsExtensions.AddProblemDetails(builder.Services);

builder.Services.AddDbContext<ProductDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionString"));
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserDeletedConsumer>();
    x.AddConsumer<UserDeactivatedConsumer>();
    x.AddConsumer<UserActivatedConsumer>();

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

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IValidator<ProductDTO>, ProductDTOValidator>();

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

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    context.Database.Migrate();
}

app.UseProblemDetails();
app.UseSwagger();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();