using ConnectDB.Data;
using ConnectDB.Models;
using Microsoft.AspNetCore.Mvc;
namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PostsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Posts.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null) return NotFound();

            return Ok(post);
        }

        [HttpPost]
        public IActionResult Create(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();

            return Ok(post);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Post post)
        {
            var existing = _context.Posts.Find(id);
            if (existing == null) return NotFound();

            existing.Title = post.Title;
            existing.Content = post.Content;
            existing.Image = post.Image;

            _context.SaveChanges();

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null) return NotFound();

            _context.Posts.Remove(post);
            _context.SaveChanges();

            return Ok();
        }
    }
}
