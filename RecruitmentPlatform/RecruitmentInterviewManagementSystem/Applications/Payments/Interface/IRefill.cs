using RecruitmentInterviewManagementSystem.Applications.Payments.DTO;

namespace RecruitmentInterviewManagementSystem.Applications.Payments.Interface
{
    public interface IRefill
    {
        Task<ResponseRefillDTO> Execute(RefillDTO request);

        Task<decimal> Execute(Guid idUser);

        Task<bool> GiftCodeBeginer(CodeBeginer code);
    }


    public class CodeBeginer
    {
        public Guid IdUser { get; set; }

        public string Code { get; set; } = string.Empty;
    }
}
