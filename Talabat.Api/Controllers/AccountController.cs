using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.Dtos;
using Talabat.Api.Errors;
using Talabat.Core.Entities.Identity;

namespace Talabat.Api.Controllers
{
    public class AccountController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            var user =await _userManager.FindByEmailAsync(login.Email);
            if (user is null) return Unauthorized(new ApiErrorResponse(401));
            var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            if(!checkPassword.Succeeded) return Unauthorized(new ApiErrorResponse(401));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = "Token Will Be Generated"
            });
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto register)
        {
            var user = await _userManager.FindByEmailAsync(register.Email);
            if (user is not null) return BadRequest(new ApiErrorResponse(400, "Email Is Already Exists"));
            var appUser = new AppUser()
            {
                DisplayName = register.DisplayName,
                UserName = register.UserName,
                Email = register.Email,
                PhoneNumber = register.PhoneNumber,
            };
            var createdUser = await _userManager.CreateAsync(appUser, register.Password);
            if (!createdUser.Succeeded) return BadRequest(new ApiErrorResponse(400));
            return Ok(new { message = "Registered Successful"});
        }
    }
}
