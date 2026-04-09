using ConnectDB.Data;
using ConnectDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Payments.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _context.Payments.Find(id);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create(Payment item)
        {
            _context.Payments.Add(item);
            _context.SaveChanges();

            return Ok(item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Payment item)
        {
            var existing = _context.Payments.Find(id);
            if (existing == null) return NotFound();

            existing.Method = item.Method;
            existing.Status = item.Status;

            _context.SaveChanges();

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _context.Payments.Find(id);
            if (item == null) return NotFound();

            _context.Payments.Remove(item);
            _context.SaveChanges();

            return Ok();
        }
    }
}
