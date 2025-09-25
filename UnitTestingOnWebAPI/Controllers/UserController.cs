using API.Models;
using API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IUserInterface userInterface) : ControllerBase
    {
        // Create
        [HttpPost("add")]

        public async Task<IActionResult> Create(User user)
        {
            var result = await userInterface.CreateAsync(user);
            if (result)
                return CreatedAtAction(nameof(Create), new { id = user.Id }, user);
            else
                return BadRequest();
        }

        // Read All
        [HttpPost("get")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await userInterface.GetAllAsync();
            if (!users.Any())
                return NotFound();
            else
                return Ok(users);
        }
    }
}
