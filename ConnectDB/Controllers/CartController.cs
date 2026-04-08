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

        public CartsController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET ALL (có include)
        [HttpGet]
        public IActionResult GetAll()
        {
            var carts = _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .ToList();

            return Ok(carts);
        }

        // ✅ GET BY ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var cart = _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.Id == id);

            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        // ✅ CREATE
        [HttpPost]
        public IActionResult Create(Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();

            return Ok(cart);
        }

        // ✅ UPDATE (ít dùng nhưng vẫn có)
        [HttpPut("{id}")]
        public IActionResult Update(int id, Cart cart)
        {
            var existing = _context.Carts.Find(id);

            if (existing == null)
                return NotFound();

            existing.UserId = cart.UserId;

            _context.SaveChanges();

            return Ok(existing);
        }

        // ✅ DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cart = _context.Carts.Find(id);

            if (cart == null)
                return NotFound();

            _context.Carts.Remove(cart);
            _context.SaveChanges();

            return Ok();
        }
    }
}