using RecruitmentInterviewManagementSystem.Applications.Payments.DTO;

namespace RecruitmentInterviewManagementSystem.Applications.Payments.Interface
{
    public interface IPayment
    {
        Task<ResponsePaymentDTO> CreatePayment(CreatePaymentDTO createPaymentDTO);
    }
}
