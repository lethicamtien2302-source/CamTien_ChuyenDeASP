using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConnectDB.Data;
using ConnectDB.Models;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductsController(AppDbContext context) => _context = context;

        // 1. Cập nhật hàm GetProducts để trả về thêm CategoryName và BrandName
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Select(p => new {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Stock,
                    p.Image,
                    p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.Name : "N/A", // Lấy tên Cate
                    p.BrandId,
                    BrandName = p.Brand != null ? p.Brand.Name : "N/A"
                })
                .ToListAsync();
            return products;
        }

        // 2. Cập nhật hàm PutProduct để xử lý Up ảnh khi Sửa
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromForm] Product product, IFormFile? imageFile)
        {
            if (id != product.Id) return BadRequest();

            try
            {
                // Lấy thông tin sản phẩm cũ trong DB để giữ lại ảnh nếu không update ảnh mới
                var existingProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                if (existingProduct == null) return NotFound();

                if (imageFile != null && imageFile.Length > 0)
                {
                    // Xử lý lưu file mới (giống hàm Post)
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(path, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    product.Image = fileName; // Lưu tên file mới
                }
                else
                {
                    // Nếu không chọn file mới, giữ nguyên tên file cũ
                    product.Image = existingProduct.Image;
                }

                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.Id == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.Include(p => p.Brand).Include(p => p.Category).Include(p => p.ProductDetail).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromForm] Product product, IFormFile? imageFile)
        {
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Xác định đường dẫn lưu file vào thư mục wwwroot/images
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                    // Tạo tên file duy nhất bằng GUID để không bị trùng
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(path, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Gán tên file vào thuộc tính Image của model Product để lưu vào DB
                    product.Image = fileName;
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}