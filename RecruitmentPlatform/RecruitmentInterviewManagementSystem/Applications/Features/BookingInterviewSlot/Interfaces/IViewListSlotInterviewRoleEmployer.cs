using RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.DTO;

namespace RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.Interfaces
{
    public interface IViewListSlotInterviewRoleEmployer
    {
        Task<ResponseInterViewSlots> Execute(RequestViewInterviewSlots requestViewInterviewSlots);
    }
}
