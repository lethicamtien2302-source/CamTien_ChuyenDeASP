using ConnectDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Quan trọng: Phải có dòng này để dùng CountAsync

namespace ConnectDB.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Sử dụng Task.WhenAll để đếm tất cả cùng lúc cho nhanh
                return Ok(new
                {
                    Products = await _context.Products.CountAsync(),
                    Users = await _context.Users.CountAsync(),
                    Orders = await _context.Orders.CountAsync(),
                    Categories = await _context.Categories.CountAsync(),
                    Brands = await _context.Brands.CountAsync(),
                    Posts = await _context.Posts.CountAsync(),
                    Contacts = await _context.Contacts.CountAsync(),
                    Payments = await _context.Payments.CountAsync()
                });
            }
            catch (System.Exception ex)
            {
                // Nếu bị lỗi, trả về thông báo để bạn biết bảng nào đang bị sai tên
                return BadRequest($"Lỗi truy vấn: {ex.Message}");
            }
        }
    }
}