using ConnectDB.Data;
using ConnectDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET ALL
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        // ✅ GET BY ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // ✅ CREATE
        [HttpPost]
        public IActionResult Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(user);
        }

        // ✅ UPDATE
        [HttpPut("{id}")]
        public IActionResult Update(int id, User user)
        {
            var existing = _context.Users.Find(id);

            if (existing == null)
                return NotFound();

            existing.Username = user.Username;
            existing.Password = user.Password;
            existing.Email = user.Email;

            _context.SaveChanges();

            return Ok(existing);
        }

        // ✅ DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok();
        }
    }
}
