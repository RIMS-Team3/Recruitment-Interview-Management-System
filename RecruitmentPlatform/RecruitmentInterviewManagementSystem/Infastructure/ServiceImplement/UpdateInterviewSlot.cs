using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.DTO;
using RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.Interfaces;
using RecruitmentInterviewManagementSystem.Domain.Entities;
using RecruitmentInterviewManagementSystem.Models;

namespace RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement
{
    public class UpdateInterviewSlot : IUpdateInterviewSlot
    {
        private readonly FakeTopcvContext _db;

        public UpdateInterviewSlot(FakeTopcvContext fakeTopcvContext)
        {
            _db  = fakeTopcvContext;
        }

        public async Task<bool> Execute(RequestUpdateInterviewSlots request)
        {
            try
            {
                
                var slot = await _db.InterviewsSlots
                    .FirstOrDefaultAsync(x => x.IdInterviewSlot == request.IdInterviewSlot);

                if (slot == null)
                    return false; 

                if (slot.IsBooked)
                    return false; 

            
                var isExist = await _db.InterviewsSlots
                    .AnyAsync(x =>
                        x.IdCompany == request.IdCompany &&
                        x.IdInterviewSlot != request.IdInterviewSlot &&
                        x.StartTime < request.EndTime &&
                        x.EndTime > request.StartTime);

                if (isExist)
                    return false; 

              
                var domain = new InterviewSlotEntity(request.StartTime, request.EndTime);

                slot.StartTime = domain.StartTime;
                slot.EndTime = domain.EndTime;

               
                var result = await _db.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
