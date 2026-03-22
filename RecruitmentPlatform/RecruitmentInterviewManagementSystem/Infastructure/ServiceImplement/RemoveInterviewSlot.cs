using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.DTO;
using RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.Interfaces;
using RecruitmentInterviewManagementSystem.Applications.Notifications.DTO;
using RecruitmentInterviewManagementSystem.Applications.Notifications.Producers;
using RecruitmentInterviewManagementSystem.Models;

namespace RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement
{
    public class RemoveInterviewSlot : IRemoveInterviewSlot
    {
        private readonly FakeTopcvContext _db;
        private readonly IInformScheduleInterivewProducer _informProducer;

        public RemoveInterviewSlot(FakeTopcvContext fakeTopcvContext, IInformScheduleInterivewProducer informProducer)
        {
            _db = fakeTopcvContext;
            _informProducer = informProducer;
        }
        public async Task<bool> Execute(RequestRemoveInterviewSlot request)
        {
            try
            {
               
                var slot = await _db.InterviewsSlots
                    .Include(x => x.Interview)
                        .ThenInclude(i => i.application)
                            .ThenInclude(a => a.Candidate)
                                .ThenInclude(c => c.User)
                    .Include(x => x.Interview)  
                        .ThenInclude(i => i.application)
                            .ThenInclude(a => a.InterviewBookingTokens)
                    .FirstOrDefaultAsync(x => x.IdInterviewSlot == request.IdInterviewSlot &&
                                              x.IdCompany == request.IdCompany);

                if (slot == null)
                    return false; 

              
                if (slot.IsBooked)
                {
                    var application = slot.Interview?.application;

                    if (application != null)
                    {
                        
                        var bookingToken = application.InterviewBookingTokens?
                            .Where(t => t.IsUsed)
                            .OrderByDescending(t => t.CreatedAt)
                            .FirstOrDefault();

                        if (bookingToken != null)
                        {
                          
                            bookingToken.IsUsed = false;

                          
                            if (bookingToken.ExpiredAt < DateTime.UtcNow.AddDays(1))
                            {
                                bookingToken.ExpiredAt = DateTime.UtcNow.AddDays(3); 
                            }
                        }

                       
                        var candidateUser = application.Candidate?.User;
                        if (candidateUser != null)
                        {
                            var notification = new NotificationDTOS
                            {
                                Name = candidateUser.FullName ?? "Ứng viên",
                                Email = candidateUser.Email,
                                Titel = "Thông báo hủy lịch phỏng vấn & Đặt lại lịch",
                                Message = $"Chào {candidateUser.FullName ?? "bạn"},\n\n" +
          $"Rất tiếc, lịch phỏng vấn của bạn vào {slot.StartTime:dd/MM/yyyy HH:mm} đã bị hủy.\n" +
          $"Bạn có thể chọn lại lịch mới tại đây:\n\n" +
          $"https://itlocak.xyz/interview-schedule/{bookingToken?.Token}\n\n" +
          $"Cảm ơn bạn đã quan tâm và mong sớm được trao đổi cùng bạn!\n\n" +
          $"Trân trọng,\nĐội ngũ tuyển dụng",
                                TypeService = "Email",
                            };

                            await _informProducer.Execute(notification);
                        }
                    }

                    if (slot.Interview != null)
                    {
                        _db.Interview.Remove(slot.Interview);
                    }
                }


                _db.InterviewsSlots.Remove(slot);

               
                var result = await _db.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
          
                return false;
            }
        }
    }
}
