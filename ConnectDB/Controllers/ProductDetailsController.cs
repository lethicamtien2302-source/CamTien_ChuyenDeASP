using ConnectDB.Data;
using ConnectDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ConnectDB.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductDetailsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _context.ProductDetails
                .Include(p => p.Product)
                .ToList();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _context.ProductDetails
                .Include(p => p.Product)
                .FirstOrDefault(p => p.Id == id);

            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create(ProductDetail item)
        {
            _context.ProductDetails.Add(item);
            _context.SaveChanges();

            return Ok(item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, ProductDetail item)
        {
            var existing = _context.ProductDetails.Find(id);
            if (existing == null) return NotFound();

            existing.Description = item.Description;
            existing.Specification = item.Specification;

            _context.SaveChanges();

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _context.ProductDetails.Find(id);
            if (item == null) return NotFound();

            _context.ProductDetails.Remove(item);
            _context.SaveChanges();

            return Ok();
        }
    }
}
