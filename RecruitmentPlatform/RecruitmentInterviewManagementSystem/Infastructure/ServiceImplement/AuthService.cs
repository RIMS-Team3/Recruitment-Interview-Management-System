using BCrypt.Net;
using RecruitmentInterviewManagementSystem.Models;
using RecruitmentInterviewManagementSystem.Infastructure.Repository;
using RecruitmentInterviewManagementSystem.Domain.InterfacesRepository;

public class AuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<string> RegisterAsync(RegisterRequest request)
    {
        // 1. Check email tồn tại
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
            throw new Exception("Email already exists");

        // 2. Tạo salt + hash password
        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

        // 3. Tạo user mới
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = passwordHash,
            Salt = salt,
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            Role = 1, // ví dụ: 1 = Candidate
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // 4. Lưu DB
        await _userRepository.AddAsync(user);

        return "Register successfully";
    }
}