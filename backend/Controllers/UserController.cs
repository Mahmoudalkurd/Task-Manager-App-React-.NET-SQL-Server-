using backend.Domain.Entities;
using backend.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(AppDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            _logger.LogInformation("Fetching all users");
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            _logger.LogInformation("Fetching user with ID {Id}", id);
            var user = await _context.Users
                .Include(u => u.Tasks)
                .ThenInclude(t => t.TaskTags)
                .ThenInclude(tt => tt.Tag)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                _logger.LogWarning("User not found with ID {Id}", id);
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            _logger.LogInformation("Creating user: {Name}", user.Name);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User created with ID {Id}", user.Id);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                _logger.LogWarning("UpdateUser ID mismatch: {Id}", id);
                return BadRequest("ID mismatch");
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("User updated with ID {Id}", id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(e => e.Id == id))
                {
                    _logger.LogWarning("User not found during update: {Id}", id);
                    return NotFound();
                }

                throw; // Global middleware will handle
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _logger.LogInformation("Deleting user with ID {Id}", id);
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID {Id}", id);
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User deleted with ID {Id}", id);

            return NoContent();
        }
    }
}
