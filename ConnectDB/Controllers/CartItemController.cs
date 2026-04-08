using Microsoft.EntityFrameworkCore;
using ConnectDB.Data;
using ConnectDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartItemsController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET ALL
        [HttpGet]
        public IActionResult GetAll()
        {
            var items = _context.CartItems
                .Include(ci => ci.Product)
                .ToList();

            return Ok(items);
        }

        // ✅ GET BY ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefault(ci => ci.Id == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // ✅ ADD ITEM
        [HttpPost]
        public IActionResult AddItem(CartItem item)
        {
            _context.CartItems.Add(item);
            _context.SaveChanges();

            return Ok(item);
        }

        // ✅ UPDATE (quantity)
        [HttpPut("{id}")]
        public IActionResult Update(int id, CartItem item)
        {
            var existing = _context.CartItems.Find(id);

            if (existing == null)
                return NotFound();

            existing.Quantity = item.Quantity;

            _context.SaveChanges();

            return Ok(existing);
        }

        // ✅ DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _context.CartItems.Find(id);

            if (item == null)
                return NotFound();

            _context.CartItems.Remove(item);
            _context.SaveChanges();

            return Ok();
        }
    }
}