using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.Dtos.Securty;
using Talabat.Api.Errors;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.Api.Controllers
{
    
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,IAuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
          _authService = authService;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>>Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized(new ApiResponseToNotFound_Badrequest_Unauthorized(401));
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if(result.Succeeded is false )
            {
                return Unauthorized(new ApiResponseToNotFound_Badrequest_Unauthorized(401));
            }
            return Ok(new UserDto()
            {
                DisplayName =user.DisplayName,
                Email=user.Email,
                Token=await _authService.CreateTokenAsync(user, _userManager)
            });
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var user = new AppUser 
            { 
               DisplayName= model.DisplayName,
               Email=model.Email,
               UserName=model.Email.Split('@')[0],
               PhoneNumber=model.PhoneNumber,
              
            };
            var result=  await _userManager.CreateAsync(user,model.Password);
            if(result.Succeeded is false )
            {
                return BadRequest(new ApiResponseToNotFound_Badrequest_Unauthorized(400));
            }
            return Ok(new UserDto()
            {
                DisplayName =user.DisplayName,
                Email=user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }

    }
}
