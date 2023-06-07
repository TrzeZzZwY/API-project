using AppCore.Models;
using Infrastructure.EF.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Dto.Input;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;
        private readonly UserManager<UserEntity> _userManager;

        public LoginController(IConfiguration config, UserManager<UserEntity> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("RegisterUser")]
        public async Task<IActionResult> Register(UserRegister user)
        {
            if (user is null)
                return BadRequest();
            //var find = await _userManager.FindByNameAsync(user.Login);
            var find = await _userManager.Users.FirstOrDefaultAsync(e => e.UserName == user.Login);
            if (find is not null)
                return BadRequest();
            UserEntity newUser = new UserEntity()
            {
                Email = user.Email,
                UserName = user.Login
            };
            var saved = await _userManager.CreateAsync(newUser, user.Password);
            await _userManager.AddToRoleAsync(newUser, "USER");
            find = await _userManager.Users.FirstOrDefaultAsync(e => e.UserName == user.Login);
            return Created(find.Id, find);
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin(UserRegister u)
        {
            if (u is null)
                return BadRequest();
            var find = await _userManager.Users.FirstOrDefaultAsync(e => e.UserName == u.Login);
            if (find is not null)
                return BadRequest();
            UserEntity newUser = new UserEntity()
            {
                Email = u.Email,
                UserName = u.Login
            };
            var saved = await _userManager.CreateAsync(newUser, u.Password);
            await _userManager.AddToRoleAsync(newUser, "ADMIN");
            find = await _userManager.Users.FirstOrDefaultAsync(e => e.UserName == u.Login);
            return Created(find.Id, find);
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLogin u)
        {
            if (u is null)
                return BadRequest();

            var user = await Authenticated(u);

            if (user is null)
                return NotFound("User not found");

            var token = await GenerateToken(user);
            return Ok(token);
        }

        private async Task<string> GenerateToken(UserEntity user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            bool isAdmin = (await _userManager.GetRolesAsync(user)).Contains("Admin");
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Role,isAdmin == true ? "Admin":"User"),
                new Claim(ClaimTypes.Email,user.Email)
            };

            var token = new JwtSecurityToken(
                _config["JwtSettings:ValidIssuer"],
                _config["JwtSettings:ValidAudience"],
                claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<UserEntity?> Authenticated(UserLogin u)
        {
            var find = await _userManager.Users.FirstOrDefaultAsync(e=> u.Login.Equals(e.UserName));
            if (find.UserName != u.Login)
                return null;

            var users = _userManager.Users.ToList();
            if (find is not null &&
                await _userManager.CheckPasswordAsync(find, u.Password))
                return find;

            return null;
        }
    }
}
