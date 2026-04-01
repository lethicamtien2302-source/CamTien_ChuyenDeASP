using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConnectDB.Data;
using ConnectDB.Models;

namespace ConnectDB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _context.Products
                .Include(x => x.Category)
                .ToList();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _context.Products
                .Include(x => x.Category)
                .FirstOrDefault(x => x.Id == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create(Product model)
        {
            _context.Products.Add(model);
            _context.SaveChanges();

            var result = _context.Products
                .Include(x => x.Category)
                .FirstOrDefault(x => x.Id == model.Id);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Product model)
        {
            var item = _context.Products.Find(id);

            if (item == null)
                return NotFound();

            item.Name = model.Name;
            item.Price = model.Price;
            item.CategoryId = model.CategoryId;

            _context.SaveChanges();

            return Ok(item);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _context.Products.Find(id);

            if (item == null)
                return NotFound();

            _context.Products.Remove(item);
            _context.SaveChanges();

            return Ok();
        }
    }
}