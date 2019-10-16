using Afsw.Command.IdentityProvider;
using Afsw.Command.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Afsw.Command.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public AccountController(
            ILogger<AccountController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        // POST: Register
        [AllowAnonymous]
        [HttpPost("Register")]
        [Consumes("application/x-www-form-urlencoded")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RegisterAsync([FromForm]RegisterModel registerModel)
        {
            var result = await _userManager.CreateAsync(
                new ApplicationUser
                {
                    UserName = registerModel.Username,
                    Email = registerModel.Email,
                },
                registerModel.Password);

            if (!result.Succeeded)
            {
                return Problem();
            }

            _logger.LogInformation($"new user {registerModel.Username} registered.");

            return Created("meow", new { registerModel.Username, registerModel.Email });
        }
    }
}