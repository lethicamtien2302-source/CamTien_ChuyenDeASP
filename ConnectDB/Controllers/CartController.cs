using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ConnectDB.Data;
using ConnectDB.Models;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CartsController(AppDbContext context) => _context = context;

        // 1. Lấy giỏ hàng theo UserId
        [HttpGet("{userId}")]
        public async Task<ActionResult<Cart>> GetCart(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems).ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            return cart == null ? NotFound() : cart;
        }
        // Thêm hàm này vào CartsController.cs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            return await _context.Carts
                .Include(c => c.User)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product) // Thêm dòng này để lấy thông tin sản phẩm
                .ToListAsync();
        }

        // 2. TẠO GIỎ HÀNG MỚI (Dùng hàm này để sửa lỗi 404/500 của bạn)
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            // Kiểm tra xem User này đã có giỏ hàng chưa để tránh trùng lặp
            var existingCart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == cart.UserId);
            if (existingCart != null) return BadRequest("User này đã có giỏ hàng rồi!");

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return Ok(cart);
        }

        // 3. Thêm món hàng vào giỏ
        [HttpPost("items")]
        public async Task<IActionResult> AddItemToCart(CartItem item)
        {
            // Kiểm tra xem CartId có tồn tại thật không
            var cartExists = await _context.Carts.AnyAsync(c => c.Id == item.CartId);
            if (!cartExists) return BadRequest("CartId không tồn tại!");

            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();
            return Ok("Đã thêm vào giỏ hàng");
        }

        [HttpPut("items/{itemId}")]
        public async Task<IActionResult> UpdateCartItem(int itemId, int quantity)
        {
            var item = await _context.CartItems.FindAsync(itemId);
            if (item == null) return NotFound();
            item.Quantity = quantity;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> RemoveCartItem(int itemId)
        {
            var item = await _context.CartItems.FindAsync(itemId);
            if (item == null) return NotFound();
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}