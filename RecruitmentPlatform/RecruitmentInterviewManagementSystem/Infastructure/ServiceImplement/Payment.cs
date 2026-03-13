using Microsoft.EntityFrameworkCore;
using PayOS;
using PayOS.Models.V2.PaymentRequests;
using RecruitmentInterviewManagementSystem.Applications.Features.Payment;
using RecruitmentInterviewManagementSystem.Applications.Features.Payment.DOT;
using RecruitmentInterviewManagementSystem.Models;

namespace RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement
{
    public class Payment : IPayment
    {
        private readonly FakeTopcvContext _db;
        private readonly PayOSClient _payos;

        public Payment(FakeTopcvContext fakeTopcvContext, PayOSClient payOSClient)
        {
            _db = fakeTopcvContext;
            _payos = payOSClient;
        }
        public async Task<ResponsePayment> Execute(CreatePaymentDTO request)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {


                var employer = await _db.EmployerProfiles.FirstOrDefaultAsync(e => e.Id == request.IdUser);


                var candidate = await _db.CandidateProfiles.FirstOrDefaultAsync(c => c.Id == request.IdUser);


                var idUser = employer?.Id ?? candidate?.Id;


                var servicePackage = await _db.ServicePackages
                    .FirstOrDefaultAsync(s => s.Id == request.IdService);

                if (servicePackage == null) return new ResponsePayment
                {
                    QRCode = string.Empty,
                    Status = false
                };


                var now = DateTime.Now;

                var orderCode = long.Parse(now.ToString("yyyyMMddHHmmss"));

                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    OrderCode = orderCode.ToString(),
                    EmployerId = idUser,
                    TotalAmount = servicePackage.Price ?? 0,
                    Status = 0,
                    CreatedAt = now,
                    CandidateId = idUser
                };

                await _db.Orders.AddAsync(order);

                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ServicePackageId = servicePackage.Id,
                    Quantity = 1,
                    Price = servicePackage.Price ?? 0
                };

                await _db.OrderItems.AddAsync(orderItem);

                await _db.SaveChangesAsync();

                var paymentRequest = new CreatePaymentLinkRequest
                {
                    OrderCode = orderCode,
                    Amount = (long)(order.TotalAmount ?? 0),
                    Description = "2HONDAICODON",
                    ReturnUrl = "https://your-url.com",
                    CancelUrl = "https://your-url.com"
                };

                var paymentLink = await _payos.PaymentRequests.CreateAsync(paymentRequest);

                if (string.IsNullOrEmpty(paymentLink?.QrCode))
                    throw new Exception("Create payment link fail");

                await transaction.CommitAsync();

                return new ResponsePayment
                {
                    QRCode = paymentLink.QrCode,
                    Status = true
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
