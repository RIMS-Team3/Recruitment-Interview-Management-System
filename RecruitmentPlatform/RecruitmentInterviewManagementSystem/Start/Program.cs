using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Applications.Features.Interface;
using RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement;
using RecruitmentInterviewManagementSystem.Models;

//cua phuc
using RecruitmentInterviewManagementSystem.Domain.InterfacesRepository;
using RecruitmentInterviewManagementSystem.Infastructure.Repository;
using RecruitmentInterviewManagementSystem.Applications.Features.JobPostDetail.Interface;


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

            // thay url của you vào file .evn 
            //Data Source=PHAMTRUNGDUC\\SQLEXPRESS;Initial Catalog=FakeTOPCV;Persist Security Info=True;User ID=sa;Password=123;Trust Server Certificate=True
            // đổi tên server and your account and password

            
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyAllowSpecificOrigins",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173") 
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });


            builder.Services.AddScoped<ILogin, Login>();
  
            builder.Services.AddScoped<IJobPostDetailRepository, JobPostDetailRepository>();
            builder.Services.AddScoped<IJobPostDetailService, JobPostDetailService>();

            builder.Services.AddControllers();

            var app = builder.Build();

        
            app.UseCors("MyAllowSpecificOrigins");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
