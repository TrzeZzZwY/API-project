using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AlbumController : Controller
    {
        [HttpGet]
        [Route("User")]
        public IActionResult UserTest()
        {
            return Ok("hello user");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Admin")]
        public IActionResult AdminTest()
        {
            return Ok("hello admin");
        }

    }
}
