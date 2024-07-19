using LogicNexBackend.Dbcontexts;
using LogicNexBackend.Models;
using LogicNexBackend.RabbitMQ;
using LogicNexBackend.Repositories;
using LogicNexBackend.Respositories;
using LogicNexBackend.SecretModels;
using LogicNexBackend.utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;
namespace LogicNexBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddControllers();
            builder.Services.Configure<MongoClientSettings>(builder.Configuration.GetSection("MongoClientSettings"));
            builder.Services.AddScoped<MongoDbContext>();
            builder.Services.AddScoped<ChannelCreator>();
            builder.Services.AddScoped<TestRepository>();
            builder.Services.AddScoped<PlanRepository>();
            builder.Services.AddScoped<ProblemSubmissionRepository>();
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<ProblemSetRepository>();
            builder.Services.AddSingleton<IEmailSender, EmailSender>();
            builder.Services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new()
                {
                    ClockSkew  = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["JWT:validIssuer"],
                    ValidAudience = builder.Configuration["JWT:validAudience"],
                    IssuerSigningKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretAuthModel.Key))
                };
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = cc =>
                    {
                        cc.Token = cc.Request.Cookies["__Host-Forbidden-Token"];
                        return Task.CompletedTask;
                    }
                };
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Allowed_Origins", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowCredentials();
                    policy.WithOrigins(["http://localhost:3000"]);
                    policy.SetPreflightMaxAge(TimeSpan.FromMinutes(10));
                });
            });
            var app = builder.Build();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            
            app.UseCors("Allowed_Origins");
            app.Run();
        }
    }
}
