using Hexa_CSTS.DTOs;
using Hexa_CSTS.Models;
using Hexa_CSTS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Hexa_CSTS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthController(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDtos.Dto>> Register([FromBody] UserDtos.Register request)
        {
            if (await _userRepository.EmailExistsAsync(request.Email))
            {
                return BadRequest("Email address already present. Please Login");
            }

            var newUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = request.Password,
                Role = UserRole.Customer,
                IsActive = true
            };

            var createdUser = await _userRepository.AddAsync(newUser);

            var userResponse = new UserDtos.Dto
            {
                UserId = createdUser.UserId,
                Name = createdUser.Name,
                Email = createdUser.Email,
                Role = createdUser.Role.ToString(),
                IsActive = createdUser.IsActive
            };

            
            return Ok(userResponse);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDtos.Login.Response>> Login([FromBody] UserDtos.Login request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || user.PasswordHash != request.Password)
            {
                return Unauthorized("Invalid email or password");
            }

            var token = CreateToken(user);

            var loginResponse = new UserDtos.Login.Response
            {
                UserId = user.UserId,
                Name = user.Name,
                Role = user.Role.ToString(),
                Token = token
            };

            return Ok(loginResponse);
        }

        private string CreateToken(User user)
        {
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = issuer,
                Audience = audience
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}