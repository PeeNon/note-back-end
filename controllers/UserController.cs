using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NotesApi.Dto;
using NotesApi.Models;
using NotesApi.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NotesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserController(UserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginModel)
        {
            if (await _userRepository.ValidateUserCredentialsAsync(loginModel.Username, loginModel.Password))
            {   
                var user = await _userRepository.GetUserByUsernameAsync(loginModel.Username);
                if(user == null)
                {
                    return Unauthorized();
                }
                var token = GenerateJwtToken(loginModel.Username, user.Id);
                return Ok(new { Token = token });
                
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerModel)
        {
            if (registerModel.Password != registerModel.PasswordConfirm)
            {
                return BadRequest("Passwords do not match.");
            }

            var existingUser = await _userRepository.GetUserByUsernameAsync(registerModel.Username);
            if (existingUser != null)
            {
                return BadRequest("Username is already taken.");
            }

            var user = new User
            {
                Username = registerModel.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(registerModel.Password)
            };

            await _userRepository.CreateUserAsync(user);

            return Ok("User registered successfully.");
        }

        private string GenerateJwtToken(string username, int userId)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                throw new InvalidOperationException("JWT configuration is missing.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", userId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}