using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ResetPassword.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace ResetPassword.Controllers
{
    [ApiController]
    [EnableCors("AllowMyOrign")]
    [Route("api/[controller]")]
    public class ResetPasswordController : ControllerBase
    {
        private UserManager<AppUser> userMgr;
        private SignInManager<AppUser> siginMgr;
        public ResetPasswordController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            userMgr = userManager;
            siginMgr = signInManager;
        }
        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel resetPassword)
        {
            Message _Message = new Message();
            IdentityResult identityResult = new IdentityResult();
            //AppUser user = new AppUser();
            try
            {
                //AppUser user = await userMgr.FindByEmailAsync("b.sriram6671@gmail.com");
                AppUser user = await userMgr.FindByIdAsync(resetPassword.Id);
                var token = await userMgr.GeneratePasswordResetTokenAsync(user);
                //var encodedtoken = Encoding.UTF8.GetBytes(token);
                //var validatetoken = WebEncoders.Base64UrlEncode(encodedtoken);
                if (user != null && user.Id == Convert.ToInt32(resetPassword.Id))
                {
                    identityResult = await userMgr.ResetPasswordAsync(user, token, resetPassword.Confirmpassword);
                    if (identityResult.Succeeded == true)
                    {
                        _Message.Status = "Success";
                        _Message.StatusCode = 200;
                        _Message.StatusMessage = "Successfully your Passwors Reset !!";
                    }

                    return Ok(_Message);
                }
                else
                {
                    _Message.Status = "Failure";
                    _Message.StatusCode = 200;
                    _Message.StatusMessage = "Passwors Reset Failed !!";
                    return Ok(_Message);
                }
            }
            catch (Exception ex)
            {
                _Message.Status = "Failure";
                _Message.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                _Message.StatusMessage = ex.Message;
            }
            return Ok(identityResult);
        }
    }
}