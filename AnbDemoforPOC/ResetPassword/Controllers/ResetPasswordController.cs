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
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using System.Web;
using System.Text.RegularExpressions;

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
            try
            {
                var user = await userMgr.FindByIdAsync(resetPassword.Id);
                if (user != null && user.Id == Convert.ToInt32(resetPassword.Id))
                {
                    var geneEmailToken = await userMgr.GenerateEmailConfirmationTokenAsync(user);
                    var response = await userMgr.ConfirmEmailAsync(user, geneEmailToken);
                    if (resetPassword.Token == user.SecurityStamp)
                    {
                        var code = await userMgr.GeneratePasswordResetTokenAsync(user);
                        var Result = await userMgr.ResetPasswordAsync(user, code, resetPassword.Confirmpassword);
                        if (Result.Succeeded == true)
                        {
                            var _UpdateSecurityStampAsync = await userMgr.UpdateSecurityStampAsync(user);
                            _Message.Status = "Success";
                            _Message.StatusCode = 200;
                            _Message.StatusMessage = "Your Password has been reset successfully.";
                        }
                        else
                        {
                            _Message.Status = "Failure";
                            _Message.StatusCode = 400;
                            _Message.StatusMessage = "Passwords must be at least 6 characters,Passwords must have at least one non alphanumeric character,Passwords must have at least one digit ('0'-'9'),Passwords must have at least one uppercase ('A'-'Z').";
                            return Ok(_Message);
                        }
                    }
                    else
                    {
                        _Message.Status = "InvalidToken";
                        _Message.StatusCode =100;
                        _Message.StatusMessage = "Invalid Token.";
                        return Ok(_Message);
                    }
                    
                    return Ok(_Message);
                }
                else
                {
                    _Message.Status = "Failure";
                    _Message.StatusCode = 300;
                    _Message.StatusMessage = "Passwors Reset Failed.";
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