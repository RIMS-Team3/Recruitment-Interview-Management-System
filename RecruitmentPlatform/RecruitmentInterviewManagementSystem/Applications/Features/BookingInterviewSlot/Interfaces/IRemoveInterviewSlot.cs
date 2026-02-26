using RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.DTO;

namespace RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.Interfaces
{
    public interface IRemoveInterviewSlot
    {
        Task<bool> Execute(RequestRemoveInterviewSlot request);
    }
}
