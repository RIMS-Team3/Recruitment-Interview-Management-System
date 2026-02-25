using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Models;
using RecruitmentInterviewManagementSystem.Domain.InterfacesRepository;

namespace RecruitmentInterviewManagementSystem.Infastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly FakeTopcvContext _context;

        public UserRepository(FakeTopcvContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}