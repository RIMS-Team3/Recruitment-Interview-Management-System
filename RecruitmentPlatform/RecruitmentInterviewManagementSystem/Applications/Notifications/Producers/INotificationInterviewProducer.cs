using RecruitmentInterviewManagementSystem.Applications.Notifications.DTO;

namespace RecruitmentInterviewManagementSystem.Applications.Notifications.Producers
{
    public interface INotificationInterviewProducer
    {
        Task Execute(NotificationDTOS request);
    }
}
