using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Webapi.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [BindProperty]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Ungültiger Username")]
        public string Username { get; set; } = default!;
        [BindProperty]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Ungültiges Passwort")]
        public string Password { get; set; } = default!;
        [FromQuery]
        public string? ReturnUrl { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, Username),
                        new Claim(ClaimTypes.Role, "admin")
                    };
            var claimsIdentity = new ClaimsIdentity(
                claims,
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(3),
            };

            await HttpContext.SignInAsync(
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return Redirect(string.IsNullOrEmpty(ReturnUrl) ? "/" : ReturnUrl);
        }
    }
}
