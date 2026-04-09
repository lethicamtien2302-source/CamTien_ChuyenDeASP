using ConnectDB.Data;
using ConnectDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AddressesController(AppDbContext context)
        {
            _context = context;
        }

        // GET ALL
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Addresses.ToList());
        }

        // GET BY ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var address = _context.Addresses.Find(id);
            if (address == null) return NotFound();

            return Ok(address);
        }

        // CREATE
        [HttpPost]
        public IActionResult Create(Address address)
        {
            _context.Addresses.Add(address);
            _context.SaveChanges();

            return Ok(address);
        }

        // UPDATE
        [HttpPut("{id}")]
        public IActionResult Update(int id, Address address)
        {
            var existing = _context.Addresses.Find(id);
            if (existing == null) return NotFound();

            existing.AddressLine = address.AddressLine;
            existing.Phone = address.Phone;

            _context.SaveChanges();

            return Ok(existing);
        }

        // DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var address = _context.Addresses.Find(id);
            if (address == null) return NotFound();

            _context.Addresses.Remove(address);
            _context.SaveChanges();

            return Ok();
        }
    }
}
