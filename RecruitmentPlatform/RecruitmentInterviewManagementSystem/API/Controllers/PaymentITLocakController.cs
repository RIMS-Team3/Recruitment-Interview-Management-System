using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecruitmentInterviewManagementSystem.Applications.Payments.DTO;
using RecruitmentInterviewManagementSystem.Applications.Payments.Interface;

namespace RecruitmentInterviewManagementSystem.API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentITLocakController : ControllerBase
    {
        private readonly IPayment _payment;

        public PaymentITLocakController(IPayment payment)
        {
            _payment = payment;
        }

        [HttpPost]
        public async Task<IActionResult> Pay([FromBody]CreatePaymentDTO request)
        {
            var result = await _payment.CreatePayment(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Payment failed");
            }

        }
        
    }
}
