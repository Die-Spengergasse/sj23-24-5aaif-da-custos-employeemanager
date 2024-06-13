using System.Linq;
using EmployeeManager.Application.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly EmployeemanagerContext _db;

        public UsersController(EmployeemanagerContext db)
        {
            _db = db;
        }

        /// <summary>
        /// We cannot access the cookie in JavaScript. To check the auth state, we can send a request
        /// to /api/user/userinfo. So we can set our application state.
        /// </summary>
        [Authorize]
        [HttpGet("userinfo")]
        public IActionResult GetAccountinfo()
        {
            var authenticated = HttpContext.User.Identity?.IsAuthenticated ?? false;
            if (!authenticated) { return Unauthorized(); }

            var username = HttpContext.User.Identity?.Name;
            var guid = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Guid")?.Value;
            var isAdmin = HttpContext.User.IsInRole("admin");

            return Ok(new
            {
                Username = username,
                Guid = guid,
                IsAdmin = isAdmin,
            });
        }
    }
}