using API.Repository;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
        [ApiController]
        [Route("[controller]")]
    public class UserController(IUserInterface userInterface) : ControllerBase
    {
        // Criar usuários
        [HttpPost("add")]
        public async Task<IActionResult> Create(User user)
        {
            var result = await userInterface.CreateAsync(user);
            if (result)
            {
                return CreatedAtAction(nameof(Create), new { id = user.Id}, user);
            }
            return BadRequest("Falha ao criar usuário.");
        }

        // Recuperar usuário por ID
        [HttpGet("get/{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await userInterface.GetByIdAsync(id);
            if (user == null)
                return NotFound("Usuário não encontrado.");
            else
                return Ok(user);
        }

        // Recuperar todos os usuários
        [HttpGet("get/all")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await userInterface.GetAllAsync();
            if (!users.Any())
                return NotFound("Nenhum usuário encontrado.");
            else
                return Ok(users);
        }

        // Atualizar usuário
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser(User user)
        {
            var result = await userInterface.UpdateAsync(user);
            if (result)
                return Ok();
            else
                return NotFound("Usuário não encontrado.");
        }

        // Deletar usuário
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await userInterface.DeleteAsync(id);
            if (result)
                return NoContent();
            else
                return NotFound("Usuário não encontrado.");
        }
    }
}
