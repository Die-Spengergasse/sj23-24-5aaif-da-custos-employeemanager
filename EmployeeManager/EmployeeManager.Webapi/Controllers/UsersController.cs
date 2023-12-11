﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
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
            return Ok(new
            {
                Username = HttpContext.User.Identity?.Name,
                IsAdmin = HttpContext.User.IsInRole("admin"),
            });
        }
    }
}
