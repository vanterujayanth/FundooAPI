using LogicLayer.Interface;
using LogicLayer.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using ReposetoryLayer.Context;
using ReposetoryLayer.Entity;
using ReposetoryLayer.Interface;
using ReposetoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static MassTransit.Logging.LogCategoryName;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
       

        var logpath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        NLog.GlobalDiagnosticsContext.Set("LogDirectory",logpath);
        builder.Logging.ClearProviders();
        builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        builder.Host.UseNLog();
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
        // builder.Services.AddDbContext<FundooContext>(x => x.UseSqlServer(builder.Configuration["ConnectionStrings:DBConnection"]));
        ////Redis cache
        //ConfigurationManager configuration = builder.Configuration;
        //builder.Services.AddStackExchangeRedisCache(options => { options.Configuration =configuration["RedisCacheUrl"]; });

        //database connection
        builder.Services.AddDbContext<FundooContext>(opts => opts.UseSqlServer(builder.Configuration["ConnectionString:FundooDBp"]));
       builder.Services.AddTransient<IUserRepo,UserRepo>();
        builder.Services.AddTransient<IUserLogic,UserLogic>();
        builder.Services.AddTransient<INotesRepo,NoteRepo>();
        builder.Services.AddTransient<INotesLogic,NotesLogic>();
        builder.Services.AddTransient<ILabelRepo,LabelRepo>();
        builder.Services.AddTransient<ILabelLogic,LabelLogic>();
        builder.Services.AddTransient<IColabRepo,ColabRepo>();
        builder.Services.AddTransient<IColabLogic,ColabLogic>();
        builder.Services.AddSwaggerGen(
    option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
    });
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var Key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SameSite = SameSiteMode.None; // or SameSiteMode.Lax or SameSiteMode.Strict
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddMassTransit(x =>
        {
            x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
            {
                config.UseHealthCheck(provider);
                config.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            }));
        });
       builder. Services.AddMassTransitHostedService();
       builder.Services.AddControllers();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseSession();
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}