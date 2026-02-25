using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using RecruitmentInterviewManagementSystem.Domain.InterfacesRepository;
using RecruitmentInterviewManagementSystem.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly FakeTopcvContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUserRepository userRepository,
        FakeTopcvContext context,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _context = context;
        _configuration = configuration;
    }

    // ================= REGISTER =================
    public async Task<string> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
            throw new Exception("Email already exists");

        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = passwordHash,
            Salt = salt,
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            Role = 1,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        return "Register successfully";
    }

    // ================= LOGIN =================
    public async Task<AuthResponse> LoginAsync(LoginUserRequest request, string ipAddress)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            throw new Exception("Invalid email or password");

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid)
            throw new Exception("Invalid email or password");

        var jwtToken = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken(user, jwtToken.Id, ipAddress);

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            RefreshToken = refreshToken.Token,
            ExpiresIn = 900
        };
    }

    // ================= REFRESH =================
    public async Task<AuthResponse> RefreshTokenAsync(string token, string ipAddress)
    {
        var refreshToken = _context.RefreshTokens
            .FirstOrDefault(x => x.Token == token);

        if (refreshToken == null ||
            refreshToken.IsRevoked == true ||
            refreshToken.IsUsed == true ||
            refreshToken.ExpiredAt <= DateTime.UtcNow)
        {
            throw new Exception("Invalid refresh token");
        }

        refreshToken.IsUsed = true;
        refreshToken.RevokedAt = DateTime.UtcNow;

        var user = refreshToken.User;

        var newJwt = GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken(user, newJwt.Id, ipAddress);

        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(newJwt),
            RefreshToken = newRefreshToken.Token,
            ExpiresIn = 900
        };
    }

    // ================= LOGOUT =================
    public async Task LogoutAsync(string refreshToken)
    {
        var token = _context.RefreshTokens
            .FirstOrDefault(x => x.Token == refreshToken);

        if (token != null)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    // ================= JWT =================
    private JwtSecurityToken GenerateJwtToken(User user)
    {
        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name, user.FullName ?? ""),
        new Claim(ClaimTypes.Role, user.Role?.ToString() ?? "1"),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds);
    }

    private RefreshToken GenerateRefreshToken(User user, string jwtId, string ipAddress)
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = Guid.NewGuid().ToString(),
            JwtId = jwtId,
            IsUsed = false,
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow,
            ExpiredAt = DateTime.UtcNow.AddDays(7),
            CreatedByIp = ipAddress
        };
    }
}