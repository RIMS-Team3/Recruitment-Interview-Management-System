using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Applications.Features.Interface;
using RecruitmentInterviewManagementSystem.Applications.Interface;
using RecruitmentInterviewManagementSystem.Applications.Features.JobPost.Interface;
using RecruitmentInterviewManagementSystem.Applications.Features.JobPost.Services;
using RecruitmentInterviewManagementSystem.Domain.InterfacesRepository;
using RecruitmentInterviewManagementSystem.Infastructure.Repository;
using RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement;
using RecruitmentInterviewManagementSystem.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RecruitmentInterviewManagementSystem.Applications.Features.Auth;
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

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IGoogleTokenValidator, GoogleTokenValidator>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<GoogleAuthService>();
            builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //JWT AUTHENTICATION

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = builder.Configuration["Authentication:Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Authentication:Jwt:Audience"],

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                builder.Configuration["Authentication:Jwt:Secret"]
                            ))
                    };
                });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            // --- 4. CẤU HÌNH HTTP REQUEST PIPELINE (MIDDLEWARE) ---
            // Chuyển hướng HTTPS (Có thể tắt nếu chạy local gặp lỗi chứng chỉ)
            app.UseHttpsRedirection();

            // Hỗ trợ file tĩnh (nếu có lưu trữ ảnh/logo trong dự án)
            app.UseStaticFiles();

            // Bật Routing trước khi dùng CORS/Auth
            app.UseRouting();

            // Kích hoạt CORS ngay sau UseRouting
            app.UseCors("AllowFrontend");

        

            // Xử lý quyền hạn
            app.UseAuthentication();
            app.UseAuthorization();

            // Map các endpoint của Controller
            app.MapControllers();

            // Chạy ứng dụng
            app.Run();
        }
    }
}