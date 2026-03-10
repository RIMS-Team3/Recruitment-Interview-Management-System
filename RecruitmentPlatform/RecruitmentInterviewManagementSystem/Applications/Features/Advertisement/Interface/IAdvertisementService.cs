using RecruitmentInterviewManagementSystem.Applications.Features.Advertisement.DTO;

namespace RecruitmentInterviewManagementSystem.Applications.Features.Advertisement.Interface
{
    public interface IAdvertisementService
    {
        Task<IEnumerable<AdvertisementDTO>> GetAllAsync();
        Task<AdvertisementDTO> GetByIdAsync(int id);
        Task<AdvertisementDTO> CreateAsync(CreateAdvertisementDTO dto);
        Task<bool> UpdateAsync(int id, UpdateAdvertisementDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
