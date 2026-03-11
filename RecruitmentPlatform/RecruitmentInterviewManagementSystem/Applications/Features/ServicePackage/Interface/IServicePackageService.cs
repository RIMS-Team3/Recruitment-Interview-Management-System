using RecruitmentInterviewManagementSystem.Applications.Features.ServicePackage.DTO;

namespace RecruitmentInterviewManagementSystem.Applications.Features.ServicePackage.Interface
{
    public interface IServicePackageService
    {
        Task<IEnumerable<ServicePackageDto>> GetAllAsync();
        Task<ServicePackageDto?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(CreateServicePackageDto request);
        Task<bool> UpdateAsync(Guid id, UpdateServicePackageDto request);
        Task<bool> DeleteAsync(Guid id);

    }
}
