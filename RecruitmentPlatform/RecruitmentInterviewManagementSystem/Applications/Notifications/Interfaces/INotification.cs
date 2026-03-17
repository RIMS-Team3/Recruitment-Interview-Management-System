using RecruitmentInterviewManagementSystem.Applications.Notifications.DTO;

namespace RecruitmentInterviewManagementSystem.Applications.Notifications.Interfaces
{
    public interface INotification
    {
        public string TypeService { get; }
        Task<bool> SendRegisterAccount(RequestSendMessage request);
    }
}
