//using Microsoft.EntityFrameworkCore;
//using RecruitmentInterviewManagementSystem.Applications.Features.Interface;
//using RecruitmentInterviewManagementSystem.Applications.Features.JobPost.Interface;
//using RecruitmentInterviewManagementSystem.Applications.Features.JobPost.Services;
//using RecruitmentInterviewManagementSystem.Domain.InterfacesRepository;
//using RecruitmentInterviewManagementSystem.Infastructure.Repository;
//using RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement;
//using RecruitmentInterviewManagementSystem.Models;

//namespace RecruitmentInterviewManagementSystem.Start
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            DotNetEnv.Env.Load();

//            var builder = WebApplication.CreateBuilder(args);

//            // 1. Cấu hình Database
//            builder.Services.AddDbContext<FakeTopcvContext>(options =>
//            {
//                options.UseSqlServer(builder.Configuration["SQLURL"]);
//            });

//            // 2. Cấu hình CORS
//            // Đặt tên chính sách đồng nhất để tránh nhầm lẫn
//            builder.Services.AddCors(options =>
//            {
//                options.AddPolicy("AllowFrontend", policy =>
//                {
//                    policy.WithOrigins("http://localhost:5173") // Port của Vite/React
//                          .AllowAnyHeader()
//                          .AllowAnyMethod()
//                          .AllowCredentials(); // Cho phép Cookie/Auth headers nếu cần
//                });
//            });

//            builder.Services.AddControllers();

//            // 3. Đăng ký Dependency Injection (DI)
//            builder.Services.AddScoped<ILogin, Login>();
//            builder.Services.AddScoped<IJobPostRepository, JobPostRepository>();
//            builder.Services.AddScoped<IViewListJobPost, ViewListJobPostService>();

//            var app = builder.Build();

//            // 4. Cấu hình HTTP request pipeline (Thứ tự Middleware rất quan trọng)

//            // Hỗ trợ xử lý file tĩnh và routing trước khi check CORS/Auth
//            app.UseStaticFiles();
//            app.UseRouting();

//            // Kích hoạt CORS ngay sau UseRouting và TRƯỚC UseAuthorization
//            app.UseCors("AllowFrontend");

//            // Nếu bạn đang chạy local và gặp lỗi SSL certificate, 
//            // bạn có thể tạm thời comment dòng UseHttpsRedirection hoặc đảm bảo máy đã tin tưởng dev-cert
//            app.UseHttpsRedirection();

//            app.UseAuthorization();

//            app.MapControllers();

//            app.Run();
//        }
//    }
//}
using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Applications.Features.Interface;
using RecruitmentInterviewManagementSystem.Applications.Features.JobPost.Interface;
using RecruitmentInterviewManagementSystem.Applications.Features.JobPost.Services;
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

            // Đăng ký các dịch vụ thuộc tầng Application (Features)
            builder.Services.AddScoped<ILogin, Login>();
            builder.Services.AddScoped<IViewListJobPost, ViewListJobPostService>();

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