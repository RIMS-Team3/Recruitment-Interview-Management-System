using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecruitmentInterviewManagementSystem.Applications.Features.Auth;
using RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.Interfaces;
using RecruitmentInterviewManagementSystem.Applications.Features.Interface;
using RecruitmentInterviewManagementSystem.Applications.Features.JobPost.Interface;
using RecruitmentInterviewManagementSystem.Applications.Features.JobPost.Services;
using RecruitmentInterviewManagementSystem.Applications.Features.JobPostDetail.Interface;
using RecruitmentInterviewManagementSystem.Applications.Interface;
using RecruitmentInterviewManagementSystem.Domain.InterfacesRepository;
using RecruitmentInterviewManagementSystem.Infastructure.Repository;
using RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement;
using RecruitmentInterviewManagementSystem.Models;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
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
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IGoogleTokenValidator, GoogleTokenValidator>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //JWT AUTHENTICATION



            var jwtSecret = builder.Configuration["Authentication:Jwt:Secret"]
                            ?? throw new Exception("JWT Secret not configured");

            var jwtIssuer = builder.Configuration["Authentication:Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Authentication:Jwt:Audience"];

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSecret)
                        ),

                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = JwtRegisteredClaimNames.Email
                    };
                });

            builder.Services.AddScoped<IJobPostDetailService, JobPostDetailService>();
            builder.Services.AddScoped<ICreateNewInterviewSlot, CreateNewInterviewSlot>();
            builder.Services.AddScoped<IUpdateInterviewSlot, UpdateInterviewSlot>();
            builder.Services.AddScoped<IRemoveInterviewSlot, RemoveInterviewSlot>();
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