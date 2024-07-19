using SignalRService.BackgroundServices;
using SignalRService.Models;
namespace SignalRService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSignalR();
            builder.Services.AddCors();
            builder.Services.AddHostedService<RabbitMQService>();
            var app = builder.Build();
            app.UseCors(x => {
                x.AllowAnyOrigin();
                x.AllowAnyHeader();
                x.AllowAnyMethod();
            });
            app.MapHub<SubmissionHub>("/Submission");
            app.Run();
        }
    }
}

