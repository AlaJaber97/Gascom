using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<BLL.Models.User> signInManager;
        private readonly UserManager<BLL.Models.User> userManager;
        public AccountController(SignInManager<BLL.Models.User> _signInManager, UserManager<BLL.Models.User> _userManager) 
        {
            this.signInManager = _signInManager;
            this.userManager = _userManager;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] BLL.Models.LoginRequest loginRequest)
        {
            var result = await signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false,false);
            if (result.Succeeded)
            {
                var user = userManager.Users.First(item => item.Email == loginRequest.Email);
                return Ok(BLL.Utils.Jwt.GenerateJwtToken(loginRequest.Email, user));
            }
            else
            {
                if (result.IsLockedOut) return Problem("your account is locked out");
                if (result.IsNotAllowed) return Problem("your account is not allowed");
                if (result.RequiresTwoFactor) return Problem("your account is requires two factor");
                return Unauthorized("email or password not correct");
            }
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] BLL.Models.User user)
        {
            user.UserName = user.Email;
            var result = await userManager.CreateAsync(user, user.Password);
            if (result.Succeeded)
            {
                return Ok(BLL.Utils.Jwt.GenerateJwtToken(user.Email, user));
            }
            else
            {
                return Problem(string.Join('\n', result.Errors));
            }
        }
    }
}
