using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.DTO;
using RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.Interfaces;
using RecruitmentInterviewManagementSystem.Models;

namespace RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement
{
    public class ViewListSlotInterviewRoleEmployer : IViewListSlotInterviewRoleEmployer
    {
        private readonly FakeTopcvContext _db;

        public ViewListSlotInterviewRoleEmployer(FakeTopcvContext fakeTopcvContext)
        {
            _db = fakeTopcvContext;
        }

        public async Task<ResponseInterViewSlots> Execute(RequestViewInterviewSlots requestViewInterviewSlots)
        {
            const int pageSize = 9;

            var company = await _db.Companies
                .FirstOrDefaultAsync(s => s.Id == requestViewInterviewSlots.IdCompany);

            if (company == null)
                throw new Exception("Company not found");

            var query = _db.InterviewsSlots
                .Where(s => s.IdCompany == requestViewInterviewSlots.IdCompany)
                .AsQueryable();

            if (requestViewInterviewSlots.ChooesDate.HasValue)
            {
                var selectedDate = requestViewInterviewSlots.ChooesDate.Value.Date;
                var nextDate = selectedDate.AddDays(1);

                query = query.Where(s =>
                    s.StartTime >= selectedDate &&
                    s.StartTime < nextDate
                );
            }

            var totalItems = await query.CountAsync();

            var numberOfPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var interviewSlot = await query
                .OrderBy(s => s.StartTime)
                .Skip((requestViewInterviewSlots.CurrentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new InterviewSlotItem
                {
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    IsBooked = s.IsBooked,
                    IdCompany = s.IdCompany
                })
                .ToListAsync();

            return new ResponseInterViewSlots
            {
                NameCompany = company.Name,
                Address = company.Address,
                UrlWebsite = company.Website,
                Description = company.Description,
                UrlLogoImage = company.LogoUrl,
                interviewSlotItems = interviewSlot,
                NumberOfPages = numberOfPages
            };
        }
    }
}
