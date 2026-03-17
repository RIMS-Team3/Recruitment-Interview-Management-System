using RecruitmentInterviewManagementSystem.Applications.Notifications.DTO;

namespace RecruitmentInterviewManagementSystem.Applications.Notifications.Producers
{
    public interface IEmailProducer
    {
        Task Execute(NotificationDTOS request);
    }
}
