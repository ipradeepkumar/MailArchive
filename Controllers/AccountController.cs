using MailArchive.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Controllers;

namespace MailArchive.WebAPI.Controllers
{
    public class AccountController : BaseController<AccountController>
    {
        public AccountController(IConfiguration config, ILogger<AccountController> log) : base(config, log)
        {
        }

       
        [HttpPost("LoginUser")]
        public IActionResult LoginUser([FromBody] UserModel user)
        {
            if (user.Password == "admin123")
            {
                responseData = SetResponse(true, PrepareToken(user), null);
            }
            else
            {
                responseData = SetResponse(false, "Incorrect password" , "Bad Credentials");
            }
            
            return Ok(responseData);
        }

        private string PrepareToken(UserModel user)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim("email", user.Email));
            claims.Add(new Claim("role", "admin"));


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("HAJSS86012940ASDJASDASDI2349872342342342340923SDFFR234023SDF90234"));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(20);
            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: cred);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
