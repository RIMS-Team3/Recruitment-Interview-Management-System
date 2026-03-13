using RecruitmentInterviewManagementSystem.Applications.Features.Payment.DOT;

namespace RecruitmentInterviewManagementSystem.Applications.Features.Payment
{
    public interface IPayment
    {
        Task<ResponsePayment> Execute(CreatePaymentDTO request);
    }
}
