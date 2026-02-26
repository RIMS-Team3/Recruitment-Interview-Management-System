using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.DTO;
using RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.Interfaces;
using RecruitmentInterviewManagementSystem.Models;

namespace RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement
{
    public class RemoveInterviewSlot : IRemoveInterviewSlot
    {
        private readonly FakeTopcvContext _db;

        public RemoveInterviewSlot(FakeTopcvContext fakeTopcvContext)
        {
            _db = fakeTopcvContext;
        }

        public async Task<bool> Execute(RequestRemoveInterviewSlot request)
        {
            try
            {
                // 1️⃣ Lấy slot từ DB
                var slot = await _db.InterviewsSlots
                    .FirstOrDefaultAsync(x =>
                        x.IdInterviewSlot == request.IdInterviewSlot &&
                        x.IdCompany == request.IdCompany);

                if (slot == null)
                    return false; // slot không tồn tại

                if (slot.IsBooked)
                    return false; // slot đã được book, không được xóa

                // 2️⃣ Xóa slot
                _db.InterviewsSlots.Remove(slot);

                // 3️⃣ Lưu DB
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
