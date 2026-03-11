using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Applications.Features.ServicePackage.DTO;
using RecruitmentInterviewManagementSystem.Applications.Features.ServicePackage.Interface;
using RecruitmentInterviewManagementSystem.Models;
using System;

namespace RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement
{
    public class ServicePackageService : IServicePackageService
    {
        private readonly FakeTopcvContext _context; 

        public ServicePackageService(FakeTopcvContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ServicePackageDto>> GetAllAsync()
        {
            return await _context.ServicePackages
                .Select(x => new ServicePackageDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    DurationDays = x.DurationDays,
                    MaxPost = x.MaxPost,
                    IsActive = x.IsActive
                }).ToListAsync();   
        }

        public async Task<ServicePackageDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.ServicePackages.FindAsync(id);
            if (entity == null) return null;

            return new ServicePackageDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                DurationDays = entity.DurationDays,
                MaxPost = entity.MaxPost,
                IsActive = entity.IsActive
            };
        }

        public async Task<Guid> CreateAsync(CreateServicePackageDto request)
        {
            var entity = new ServicePackage
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                DurationDays = request.DurationDays,
                MaxPost = request.MaxPost,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.ServicePackages.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateServicePackageDto request)
        {
            var entity = await _context.ServicePackages.FindAsync(id);
            if (entity == null) return false;

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.Price = request.Price;
            entity.DurationDays = request.DurationDays;
            entity.MaxPost = request.MaxPost;
            entity.IsActive = request.IsActive;

            _context.ServicePackages.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.ServicePackages.FindAsync(id);
            if (entity == null) return false;

            _context.ServicePackages.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
