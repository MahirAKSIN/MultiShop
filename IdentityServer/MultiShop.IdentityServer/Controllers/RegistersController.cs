using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MultiShop.IdentityServer.Dtos;
using MultiShop.IdentityServer.Models;

namespace MultiShop.IdentityServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegistersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UserRegister(UserRegisterDto userRegisterDto) 
        {
            var values = new ApplicationUser
            {
                UserName = userRegisterDto.UserName,
                Email = userRegisterDto.Email,
                Surname = userRegisterDto.UserName,
                Name = userRegisterDto.UserName,

            };
            var result = await _userManager.CreateAsync(values, userRegisterDto.Password);
            if (result.Succeeded)
            {
                return Ok("User Add succesed");
            }
            else
            {
                return Ok("User Add error");
            }



        }

    }
}
