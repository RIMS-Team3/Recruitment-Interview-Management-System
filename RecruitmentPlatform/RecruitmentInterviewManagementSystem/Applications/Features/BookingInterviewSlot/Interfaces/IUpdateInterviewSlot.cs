using RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.DTO;

namespace RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.Interfaces
{
    public interface IUpdateInterviewSlot
    {
        public Task<bool> Execute(RequestUpdateInterviewSlots request);
    }
}
