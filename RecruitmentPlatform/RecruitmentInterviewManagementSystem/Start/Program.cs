using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.Interfaces;
using RecruitmentInterviewManagementSystem.Applications.Features.Interface;
using RecruitmentInterviewManagementSystem.Applications.Features.JobPost.Interface;
using RecruitmentInterviewManagementSystem.Applications.Features.JobPost.Services;
using RecruitmentInterviewManagementSystem.Applications.Features.JobPostDetail.Interface;
using RecruitmentInterviewManagementSystem.Domain.InterfacesRepository;
using RecruitmentInterviewManagementSystem.Infastructure.Repository;
using RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement;
using RecruitmentInterviewManagementSystem.Models;




namespace RecruitmentInterviewManagementSystem.Start
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Load biến môi trường từ file .env (SQLURL, v.v.)
            DotNetEnv.Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            // --- 1. CẤU HÌNH DATABASE ---
            builder.Services.AddDbContext<FakeTopcvContext>(options =>
            {
                // Lấy chuỗi kết nối từ biến môi trường
                options.UseSqlServer(builder.Configuration["SQLURL"]);
            });

            // --- 2. CẤU HÌNH CORS ---
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            // Thêm dịch vụ Controller
            builder.Services.AddControllers();

            // --- 3. ĐĂNG KÝ DEPENDENCY INJECTION (DI) ---
            // Đăng ký Repository để Controller có thể gọi được _jobPostRepository
            builder.Services.AddScoped<IJobPostRepository, JobPostRepository>();
            builder.Services.AddScoped<IJobPostDetailRepository, JobPostDetailRepository>();


            // Đăng ký các dịch vụ thuộc tầng Application (Features)
            builder.Services.AddScoped<ILogin, Login>();
            builder.Services.AddScoped<IViewListJobPost, ViewListJobPostService>();
            builder.Services.AddScoped<IViewListSlotInterviewRoleEmployer, ViewListSlotInterviewRoleEmployer>();

            builder.Services.AddScoped<IJobPostDetailService, JobPostDetailService>();

            var app = builder.Build();

            // --- 4. CẤU HÌNH HTTP REQUEST PIPELINE (MIDDLEWARE) ---

            // Hỗ trợ file tĩnh (nếu có lưu trữ ảnh/logo trong dự án)
            app.UseStaticFiles();

            // Bật Routing trước khi dùng CORS/Auth
            app.UseRouting();

            // Kích hoạt CORS ngay sau UseRouting
            app.UseCors("AllowFrontend");

            // Chuyển hướng HTTPS (Có thể tắt nếu chạy local gặp lỗi chứng chỉ)
            app.UseHttpsRedirection();

            // Xử lý quyền hạn
            app.UseAuthorization();

            // Map các endpoint của Controller
            app.MapControllers();

            // Chạy ứng dụng
            app.Run();
        }
    }
}