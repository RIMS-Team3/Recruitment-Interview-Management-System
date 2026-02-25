using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Applications.Features.Interface;
using RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement;
using RecruitmentInterviewManagementSystem.Infastructure.Repository;
using RecruitmentInterviewManagementSystem.Domain.InterfacesRepository;
using RecruitmentInterviewManagementSystem.Models;

namespace RecruitmentInterviewManagementSystem.Start
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            // ================= DATABASE =================
            builder.Services.AddDbContext<FakeTopcvContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["SQLURL"]);
            });

            // ================= CORS (FIXED) =================
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173") // ⚠ sửa port ở đây
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            // ================= DEPENDENCY INJECTION =================
            builder.Services.AddScoped<ILogin, Login>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<AuthService>();

            // ================= CONTROLLERS =================
            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseCors("AllowFrontend"); // ⚠ phải trước Authorization

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}