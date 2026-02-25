using Microsoft.EntityFrameworkCore;
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


            builder.Services.AddDbContext<FakeTopcvContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["SQLURL"]);
            });

            // thay url của you vào file .env
            //Data Source=PHAMTRUNGDUC\\SQLEXPRESS;Initial Catalog=FakeTOPCV;Persist Security Info=True;User ID=sa;Password=123;Trust Server Certificate=True
            // đổi tên server and your account and password


       
            builder.Services.AddScoped<ILogin, Login>();


            builder.Services.AddControllers();

            var app = builder.Build();

           

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
