using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecruitmentInterviewManagementSystem.Applications.Features.Payment;
using RecruitmentInterviewManagementSystem.Applications.Features.Payment.DOT;

namespace RecruitmentInterviewManagementSystem.API.Controllers
{
    [Authorize]
    [Route("api/payment")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPayment _ipayment;

        public PaymentsController(IPayment payment)
        {
            _ipayment = payment;
        }

        [HttpPost]
        public async Task<IActionResult> CreatesPayment([FromBody]CreatePaymentDTO request)
        {
            var result = await _ipayment.Execute(request);

            return Ok(result);
        }
    }
}
