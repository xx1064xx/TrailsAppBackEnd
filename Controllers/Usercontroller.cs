using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrailsAppRappi.Core;
using TrailsAppRappi.Core.Entities;
using TrailsAppRappi.Data;
using TrailsAppRappi.Interfaces;

namespace TrailsAppRappi.Controllers
{
    public class LoginInfo
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class RegisterInfo
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class UserInfo
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
    public class UserToken
    {
        public string Email { get; set; }
        public string JWT { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IDbContextFactory contextFactory;

        public UsersController(IDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterInfo login)
        {

            using var context = contextFactory.CreateContext();

            User userInDb = context.Users.FirstOrDefault(user => user.Email == login.Email);

            if (userInDb == null)
            {
                string salt;

                string pwHash = HashGenerator.GenerateHash(login.Password, out salt);
                User newUser = new User()
                {
                    FirstName = login.Firstname,
                    LastName = login.Lastname,
                    Email = login.Email,
                    Password = pwHash,
                    Salt = salt,
                };
                context.Users.Add(newUser);

                context.SaveChanges();

                return Ok(CreateToken(newUser.UserId, newUser.Email));
            }

            return BadRequest();
        }

        [HttpPost("login")]
        public IActionResult Login(LoginInfo login)
        {

            using var context = contextFactory.CreateContext();

            User userInDb = context.Users.FirstOrDefault(user => user.Email == login.Email);

            if (userInDb != null
                && HashGenerator.VerifyHash(userInDb.Password, login.Password, userInDb.Salt))
            {
                return Ok(CreateToken(userInDb.UserId, userInDb.Email));
            }
            return Unauthorized();
        }


        [Authorize]
        [HttpGet("getCurrentUser")]
        public IActionResult getCurrentUser()
        {

            Claim subClaim = User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
            string userId = Convert.ToString(subClaim.Value);

            using var context = contextFactory.CreateReadOnlyContext();

            try
            {
               
                UserInfo user = context.Users
                    .Where(u => u.UserId.ToString() == userId)
                    .Select(s => new UserInfo
                    {
                        Firstname = s.FirstName,
                        Lastname = s.LastName
                    })
                    .FirstOrDefault();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        [Authorize]
        [HttpDelete("delete")]
        public IActionResult DeleteUser()
        {

            using var context = contextFactory.CreateContext();

            var userId = GetUserIdFromToken();

            if (userId != null)
            {
                var user = context.Users.FirstOrDefault(u => u.UserId == userId);

                if (user != null)
                {

                    // Lösche den Benutzer
                    context.Users.Remove(user);

                    context.SaveChanges();
                    return Ok("Benutzer erfolgreich gelöscht");
                }

                return NotFound("Benutzer nicht gefunden");
            }

            return Unauthorized("Ungültiges Token");
        }

        private Guid? GetUserIdFromToken()
        {
            Claim subClaim = User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
            Guid userId = Guid.Parse(subClaim.Value);

            return userId;
        }

        private UserToken CreateToken(Guid userId, string email)
        {
            var expires = DateTime.UtcNow.AddDays(5);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, $"{userId}"),
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = expires,
                Issuer = JwtConfiguration.ValidIssuer,
                Audience = JwtConfiguration.ValidAudience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(JwtConfiguration.IssuerSigningKey)),
                        SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            return new UserToken { Email = email, ExpiresAt = expires, JWT = jwtToken };
        }
    }

}
