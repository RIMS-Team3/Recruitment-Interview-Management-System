using FluentValidation;
using RecruitmentInterviewManagementSystem.Applications.Features.Auth.DTO;

namespace RecruitmentInterviewManagementSystem.Infastructure.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống.")
                .MinimumLength(8).WithMessage("Mật khẩu phải từ 8 ký tự trở lên.")
                .Matches(@"[A-Z]").WithMessage("Mật khẩu phải có ít nhất 1 chữ cái viết hoa.")
                .Matches(@"[a-z]").WithMessage("Mật khẩu phải có ít nhất 1 chữ cái viết thường.")
                .Matches(@"[0-9]").WithMessage("Mật khẩu phải có ít nhất 1 chữ số.")
                .Matches(@"[\!\?\*\.]").WithMessage("Mật khẩu phải có ít nhất 1 ký tự đặc biệt (!?*.).");
        }
    }
}
