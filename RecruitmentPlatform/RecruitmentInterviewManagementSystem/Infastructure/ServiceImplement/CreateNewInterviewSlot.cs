using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.DTO;
using RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.Interfaces;
using RecruitmentInterviewManagementSystem.Domain.Entities;
using RecruitmentInterviewManagementSystem.Infastructure.Models;
using RecruitmentInterviewManagementSystem.Models;

namespace RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement
{
    public class CreateNewInterviewSlot : ICreateNewInterviewSlot
    {
        private readonly FakeTopcvContext _db;
        private readonly ILogger<CreateNewInterviewSlot> _logger;

        public CreateNewInterviewSlot(FakeTopcvContext fakeTopcvContext , ILogger<CreateNewInterviewSlot> logger) 
        {
            _db = fakeTopcvContext;
            _logger = logger;
        }

        public async Task<bool> Execute(RequestCreateNewInterviewSlotDTO request)
        {
            try
            {
                // 1️⃣ Tạo domain entity (validate nằm trong constructor)
                var interviewSlotDomain = new InterviewSlotEntity(
                    request.StartTime,
                    request.EndTime
                );

                // 2️⃣ Check trùng slot trong cùng công ty
                var isExist = await _db.InterviewsSlots
                    .AnyAsync(x =>
                        x.IdCompany == request.IdCompany &&
                        x.StartTime < interviewSlotDomain.EndTime &&
                        x.EndTime > interviewSlotDomain.StartTime);

                if (isExist)
                    return false;

                // 3️⃣ Map sang EF entity
                var newInterviewSlot = new InterviewsSlots
                {
                    IdInterviewSlot = interviewSlotDomain.Id,
                    StartTime = interviewSlotDomain.StartTime,
                    EndTime = interviewSlotDomain.EndTime,
                    IsBooked = interviewSlotDomain.IsBooked,
                    IdCompany = request.IdCompany
                };

                // 4️⃣ Save DB
                await _db.InterviewsSlots.AddAsync(newInterviewSlot);
                var result = await _db.SaveChangesAsync();

                _logger.LogInformation("The Slot has been created");
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Create new slot fail {ex}");
                return false;
            }
        }
    }
}
