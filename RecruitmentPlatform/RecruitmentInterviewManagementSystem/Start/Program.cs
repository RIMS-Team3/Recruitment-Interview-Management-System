using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Applications.Interface;
using RecruitmentInterviewManagementSystem.Applications.Features.Auth;
using RecruitmentInterviewManagementSystem.Applications.Features.Interface;
using RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement;
using RecruitmentInterviewManagementSystem.Models;

namespace RecruitmentInterviewManagementSystem.Start
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            // Database
            builder.Services.AddDbContext<FakeTopcvContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["SQLURL"]);
            });

            // Services
            builder.Services.AddScoped<ILogin, Login>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //GOOGLE LOGIN
            builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();

            builder.Services.AddControllers();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Middleware
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}