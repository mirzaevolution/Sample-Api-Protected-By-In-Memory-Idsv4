using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Sample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PingController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                name = User.Identity.Name,
                email = User.FindFirstValue(JwtClaimTypes.Email),
                role = User.FindFirstValue(JwtClaimTypes.Role)
            });
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult Post()
        {
            return Ok(new
            {
                name = User.Identity.Name,
                email = User.FindFirstValue(JwtClaimTypes.Email),
                role = User.FindFirstValue(JwtClaimTypes.Role)
            });
        }
    }
}
