using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GymCollab.Controllers
{
    /// <summary>
    /// Controller responsible for handling authentication, login, logout, 
    /// and access denial logic for the GymCollab application.
    /// </summary>
    public class AuthController : Controller
    {
        /// <summary>
        /// Displays the login view where users can enter their credentials.
        /// </summary>
        /// <returns>Login view.</returns>
        public IActionResult Login() => View();

        /// <summary>
        /// Handles login requests. Validates the provided username and password 
        /// against configured admin credentials. If valid, signs the user in with claims.
        /// </summary>
        /// <param name="username">The entered username.</param>
        /// <param name="password">The entered password.</param>
        /// <returns>
        /// Redirects to the Home/Index view if login is successful, 
        /// otherwise returns the login view with an error message.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var cfg = HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetSection("Admin");
            if (username == cfg["Username"] && password == cfg["Password"])
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.Role, "Admin") };
                var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Invalid credentials";
            return View();
        }

        /// <summary>
        /// Logs the currently authenticated user out and redirects to the home page.
        /// </summary>
        /// <returns>Redirect to Home/Index after logout.</returns>
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Displays a simple access denied message when a user attempts 
        /// to access unauthorized resources.
        /// </summary>
        /// <returns>Content result with "Access denied".</returns>
        public IActionResult Denied() => Content("Access denied");
    }
}
