using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp_API.Data;
using DatingApp_API.DTOs;
using DatingApp_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
            
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userDTO){
            
            userDTO.Name = userDTO.Name.ToLower();

            if(await _repo.UserExists(userDTO.Name))return BadRequest("Username Already Exists");

            var userToCreate = new User{
                Name = userDTO.Name
            };

            var CreatedUser = await _repo.Register(userToCreate,userDTO.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO userDTO){
            var DBUser = await _repo.Login(userDTO.Name, userDTO.Password);

            if(DBUser == null) return Unauthorized();

            var claim = new[] {
                new Claim(ClaimTypes.Name.ToLower(), DBUser.Name),
                new Claim(ClaimTypes.NameIdentifier, DBUser.Id.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });


        }

    }
}