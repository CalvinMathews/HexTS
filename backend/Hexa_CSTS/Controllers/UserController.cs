using Hexa_CSTS.DTOs;
using Hexa_CSTS.Models;
using Hexa_CSTS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hexa_CSTS.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITicketRepository _ticketRepository;

        public UsersController(IUserRepository userRepository, ITicketRepository ticketRepository)
        {
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
        }

        private static UserDtos.Dto ToUserDto(User user) => new()
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            IsActive = user.IsActive
        };

        [HttpGet]
        public async Task<ActionResult<List<UserDtos.Dto>>> Get()
        {
            var users = await _userRepository.GetAllUsers();
            return Ok(users.Select(ToUserDto).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDtos.Dto>> Get(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(ToUserDto(user));
        }
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UserDtos.AdminUpdate request)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // just status here
            user.IsActive = request.IsActive;

            await _userRepository.UpdateAsync(user);
            return NoContent();
        }

       
        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UserDtos.ChangeRole request)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (!Enum.TryParse<UserRole>(request.Role, true, out var newRole))
            {
                return BadRequest("Invalid role.");
            }

            user.Role = newRole;
            await _userRepository.UpdateAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _userRepository.ExistsAsync(id))
            {
                return NotFound();
            }

            await _userRepository.DeleteAsync(id);
            return NoContent();
        }

     
    }
}