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

    public class TrailToGet
    {
        public Guid TrailId { get; set; }
        public string? Location { get; set; }
        public string? Name { get; set; }
        public DateTime? DateAndTime { get; set; }
    }

    public class TrailToCreate
    {
        public string? Location { get; set; }
        public string? Name { get; set; }
        public DateTime? DateAndTime { get; set; }
    }
    

    [Route("api/[controller]")]
    [ApiController]
    public class TrailController : ControllerBase
    {
        private readonly IDbContextFactory contextFactory;

        public TrailController(IDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        [Authorize]
        [HttpPost("createTrail")]
        public IActionResult createTrail(TrailToCreate trail)
        {
            try
            {
                Claim subClaim = User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
                Guid userId = Guid.Parse(subClaim.Value);

                using var context = contextFactory.CreateContext();

                Trail newTrail = new Trail();

                newTrail.Location = trail.Location;
                newTrail.Name = trail.Name;
                newTrail.DateAndTime = trail.DateAndTime;
                newTrail.User = context.Users.FirstOrDefault(u => u.UserId == userId);

                context.Trails.Add(newTrail);
                context.SaveChanges();

                return Ok();

            }
            catch
            {
                return BadRequest();
            }

        }

        [Authorize]
        [HttpGet("getTrail")]
        public IActionResult getTrail()
        {

            Claim subClaim = User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
            string userId = Convert.ToString(subClaim.Value);

            using var context = contextFactory.CreateContext();

            List<TrailToGet> trails = new List<TrailToGet>();

            trails = context.Trails
                .Where(x => x.UserId.ToString() == userId)
                .Select(s => new TrailToGet
                {
                    TrailId = s.TrailId,
                    Location = s.Location,
                    Name = s.Name,
                    DateAndTime = s.DateAndTime,
                })
                .ToList();

            if (trails != null)
            {
                return Ok(trails);
            }

            /*
            using var context = contextFactory.CreateContext();

            Trail trailInDb = context.Trails.FirstOrDefault(user => user.Email == login.Email);

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
            }*/

            return BadRequest();
        }

        [Authorize]
        [HttpDelete("delete/{trailId}")]
        public IActionResult DeleteUser(string trailId)
        {

            using var context = contextFactory.CreateContext();

            if (trailId != null)
            {
                Trail trail = context.Trails.FirstOrDefault(u => u.TrailId.ToString() == trailId);

                if (trail != null)
                {

                    context.Trails.Remove(trail);

                    context.SaveChanges();
                    return Ok();
                }

                return NotFound("Trail not found");
            }

            return BadRequest();

        }


    }
}
