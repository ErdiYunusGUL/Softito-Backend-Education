using CineCompare.Core.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CineCompare.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // HATA BURADAYDI: <IdentityUser> eklendi
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        // HATA BURADAYDI: <IdentityUser> eklendi
        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        // HATA BURADAYDI: <IActionResult> eklendi
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
            // Identity şifreyi otomatik Hash'ler (Tuzlar) ve veritabanına öyle yazar
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
                return Ok(new { Message = "Kullanıcı başarıyla oluşturuldu!" });

            return BadRequest(result.Errors);
        }

        // HATA BURADAYDI: <IActionResult> eklendi
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            // Kullanıcı var mı ve şifresi doğru mu?
            if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                // Şifre doğruysa Token'ın içine gömeceğimiz bilgileri (Claims) hazırlıyoruz
                var authClaims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                // JWT Token'ı yaratıyoruz (3 Saat geçerli)
                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                });
            }

            return Unauthorized(new { Message = "E-posta veya şifre hatalı!" });
        }
    }
}