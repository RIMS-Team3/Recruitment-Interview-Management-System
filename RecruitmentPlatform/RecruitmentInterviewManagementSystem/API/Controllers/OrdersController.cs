using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentInterviewManagementSystem.Applications.Features.Order.Interface;
using System.Security.Claims;
using System;
using System.Threading.Tasks;

namespace RecruitmentInterviewManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // 1. Lấy UserId từ Token theo đúng mẫu bạn muốn
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // 2. Ép kiểu sang Guid để truyền xuống Service
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized(new { message = "Không tìm thấy thông tin xác thực UserId hợp lệ trong Token." });
            }

            // 3. Gọi thẳng Service
            var pagedOrders = await _orderService.GetMyOrdersAsync(userId, pageNumber, pageSize);

            return Ok(pagedOrders);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            // 1. Lấy UserId từ Token theo đúng mẫu bạn muốn
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // 2. Ép kiểu sang Guid
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized(new { message = "Không tìm thấy thông tin xác thực." });
            }

            // 3. Lấy chi tiết đơn hàng
            var order = await _orderService.GetOrderDetailsByIdAsync(id, userId);

            if (order == null)
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng này hoặc bạn không có quyền truy cập." });
            }

            return Ok(order);
        }
    }
}