using Microsoft.EntityFrameworkCore;
using PayOS;
using PayOS.Models.V2.PaymentRequests;
using RecruitmentInterviewManagementSystem.Applications.Payments.DTO;
using RecruitmentInterviewManagementSystem.Applications.Payments.Interface;
using RecruitmentInterviewManagementSystem.Domain.Enums;
using RecruitmentInterviewManagementSystem.Models;

namespace RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement
{
    public class PaymentService : IPayment
    {
        private readonly FakeTopcvContext _db;
        private readonly PayOSClient _payos;

        public PaymentService(FakeTopcvContext fakeTopcvContext, PayOSClient payOSClient)
        {
            _db = fakeTopcvContext;
            _payos = payOSClient;
        }

        public async Task<ResponsePaymentDTO> CreatePayment(CreatePaymentDTO createPaymentDTO)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var orderCode = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));

                var service = await _db.ServicePackages
                    .FirstOrDefaultAsync(x => x.Id == createPaymentDTO.IdService);

                if (service == null)
                {
                    return new ResponsePaymentDTO
                    {
                        IsSuccess = false,
                        Message = "Service not found"
                    };
                }

                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    OrderCode = orderCode.ToString(),
                    TotalAmount = service.Price,
                    Status = 0,
                    UserId = createPaymentDTO.IdUser,
                    PaidAt = null,
                    OrderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ServicePackageId = createPaymentDTO.IdService,
                    Quantity = 1,
                    Price = service.Price ?? 0
                }
            }
                };

                var payment = new Payment
                {
                    Id = Guid.NewGuid(),
                    Amount = service.Price,
                    PaymentMethod = (int)PaymentMethods.BankTransfer,
                    Status = (int)PaymentStatus.Pending,
                    PaidAt = null,
                    OrderId = order.Id
                };

                _db.Orders.Add(order);
                _db.Payments.Add(payment);

                await _db.SaveChangesAsync();

                var paymentRequest = new CreatePaymentLinkRequest
                {
                    OrderCode = orderCode,
                    Amount = (long)order.TotalAmount,
                    Description = "CVPRO",
                    ReturnUrl = "https://your-url.com",
                    CancelUrl = "https://your-url.com"
                };

                var paymentLink = await _payos.PaymentRequests.CreateAsync(paymentRequest);

                if (paymentLink.QrCode == null)
                {
                    await transaction.RollbackAsync();

                    return new ResponsePaymentDTO
                    {
                        IsSuccess = false,
                        Message = "Failed to create payment link"
                    };
                }

                await transaction.CommitAsync();

                return new ResponsePaymentDTO
                {
                    IsSuccess = true,
                    Message = "Payment link created successfully",
                    OrderCode = paymentLink.QrCode
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
