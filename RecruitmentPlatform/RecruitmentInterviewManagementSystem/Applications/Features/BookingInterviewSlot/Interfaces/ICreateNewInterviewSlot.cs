using RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.DTO;

namespace RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.Interfaces
{
    public interface ICreateNewInterviewSlot
    {
        Task<bool> Execute(RequestCreateNewInterviewSlotDTO request);
    }
}
