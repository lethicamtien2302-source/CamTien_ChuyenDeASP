using ConnectDB.Data;
using ConnectDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BrandsController(AppDbContext context)
        {
            _context = context;
        }

        // GET ALL
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Brands.ToList());
        }

        // GET BY ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand == null) return NotFound();

            return Ok(brand);
        }

        // CREATE
        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            _context.Brands.Add(brand);
            _context.SaveChanges();
            return Ok(brand);
        }

        // UPDATE
        [HttpPut("{id}")]
        public IActionResult Update(int id, Brand brand)
        {
            var existing = _context.Brands.Find(id);
            if (existing == null) return NotFound();

            existing.Name = brand.Name;

            _context.SaveChanges();
            return Ok(existing);
        }

        // DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand == null) return NotFound();

            _context.Brands.Remove(brand);
            _context.SaveChanges();

            return Ok();
        }
    }
}
