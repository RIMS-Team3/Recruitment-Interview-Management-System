using System.Security.Cryptography.X509Certificates;

namespace RecruitmentInterviewManagementSystem.Applications.Payments.DTO
{
    public class ResponsePaymentDTO
    {
        public bool IsSuccess { get; set; }

        public string OrderCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;


    }
}
