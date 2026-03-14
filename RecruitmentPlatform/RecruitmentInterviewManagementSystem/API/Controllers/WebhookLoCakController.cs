using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayOS.Models.Webhooks;
using PayOS;
using RecruitmentInterviewManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Domain.Enums;
using RecruitmentInterviewManagementSystem.Infastructure.HubPayment;
using Microsoft.AspNetCore.SignalR;

namespace RecruitmentInterviewManagementSystem.API.Controllers
{
    [Route("/payments")]
    [ApiController]
    public class WebhookLoCakController : ControllerBase
    {
        private readonly PayOSClient _payOSClient;
        private readonly ILogger<WebhookLoCakController> _logger;
        private readonly FakeTopcvContext _db;
        private readonly IHubContext<PaymentHub> _paymentHub;

        public WebhookLoCakController(PayOSClient payOSClient, ILogger<WebhookLoCakController> logger, FakeTopcvContext fakeTopcvContext, IHubContext<PaymentHub> paymentHub)
        {
            _payOSClient = payOSClient;
            _logger = logger;
            _db = fakeTopcvContext;
            _paymentHub = paymentHub;
        }

        [HttpPost("payos/webhook")]
        public async Task<IActionResult> WebHookAsync([FromBody] Webhook webhook)
        {
            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var webhookData = await _payOSClient.Webhooks.VerifyAsync(webhook);

                if (webhookData.Code == "00" && webhookData.Description == "CVPRO")
                {
                    await _paymentHub.Clients.All.SendAsync("PaidOrder", $"Bạn đã thành toán thành công {webhookData.Amount}");
                    var order = await _db.Orders.FirstOrDefaultAsync(o => o.OrderCode == webhookData.OrderCode.ToString());
                    if (order != null)
                    {
                        var payment = await _db.Payments.FirstOrDefaultAsync(p => p.OrderId == order.Id);
                        if (payment != null) payment.Status = (int)PaymentStatus.Success;

                        order.Status = (int)PaymentStatus.Success;
                        order.PaidAt = DateTime.UtcNow;

                        var Candidate = await _db.CandidateProfiles.FirstOrDefaultAsync(c => c.UserId == order.UserId);
                        if (Candidate != null) Candidate.IsCvPro = true;
                        await _db.SaveChangesAsync();
                        await _paymentHub.Clients.All.SendAsync("PaidOrder", $"Bạn đã thành toán thành công {webhookData.Amount}");
                        await transaction.CommitAsync();

                    }

                }
                else if (webhookData.Code == "00" && webhookData.Description == "CVPRO_RENEW")
                {

                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error processing PayOS webhook");
            }
            return Ok();
        }

        [HttpGet("payos/webhook")]
        public async Task<IActionResult> WebHookAsyncTest()
        {

            return Ok();
        }
    }
}
