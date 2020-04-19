using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Register.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using System.Web;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Register.Controllers
{
    [ApiController]
    [EnableCors("AllowMyOrign")]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private UserManager<AppUser> userMgr;
        private SignInManager<AppUser> siginMgr;
        public RegisterController(IConfiguration configuration, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _configuration = configuration;
            userMgr = userManager;
            siginMgr = signInManager;
        }
        public IConfiguration Configuration { get; }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(AppUser appUser)
        {
            Message _Message = new Message();
            try
            {
                AppUser user = await userMgr.FindByEmailAsync(appUser.Email);
                AppUser checkwithusername = await userMgr.FindByNameAsync(appUser.UserName);

                if (checkwithusername != null)
                {
                    _Message.Status = "Failure";
                    _Message.StatusCode = 102;
                    _Message.StatusMessage = "User already existed with this User Name.";
                }
                else if (user != null)
                {
                    _Message.Status = "Failure";
                    _Message.StatusCode = 101;
                    _Message.StatusMessage = "User already existed with this Email Id.";
                }
                else
                {
                    user = new AppUser
                    {
                        UserName = appUser.UserName,
                        Email = appUser.Email,
                        FirstName = appUser.FirstName,
                        LastName = appUser.LastName
                    };
                    IdentityResult result = await userMgr.CreateAsync(user, "Test123!");
                    if (result.Succeeded == true)
                    {
                        user = await userMgr.FindByIdAsync(user.Id.ToString());
                        var code = user.SecurityStamp;
                        string configcallbackUrl = _configuration.GetValue<string>("callbackUrl");
                        var callbackUrl = configcallbackUrl + user.Id + "&code=" + code;
                        var subject = "Reset password request.";
                        var body = "<html><body>" +
                                   $"<h3>Hi {user.FirstName.ToUpper().ToString()}!</h3>" +
                                   $"<h3>Your account has been created , please click below link to reset your password</h3>" +
                                   $"<a  href=\"" + callbackUrl + "\"> Reset password </a>" +
                                   $"<h4>Thanks</h4>" +
                        "</body></html>";
                        var message = new IdentityMessage
                        {
                            Body = body,
                            Destination = user.Email,// added by mallesh
                            Subject = subject
                        };
                        MailMessage msg = new MailMessage();
                        msg.From = new MailAddress("sriram.biccavolu@anblicks.com");
                        msg.To.Add(new MailAddress(message.Destination));
                        msg.Subject = message.Subject;
                        msg.Body = message.Body;
                        msg.IsBodyHtml = true;
                        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587))
                        {
                            UseDefaultCredentials = false
                        };
                        System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("sriram.biccavolu@anblicks.com", "sriram@123");
                        smtpClient.Credentials = credentials;
                        smtpClient.EnableSsl = true;
                        smtpClient.Send(msg);
                        smtpClient.Dispose();
                        _Message.Status = "Success";
                        _Message.StatusCode = 200;
                        _Message.StatusMessage = "User successfully created.";
                    }
                    else
                    {
                        _Message.Status = "Failure";
                        _Message.StatusCode = 103;
                        _Message.StatusMessage = "User registration failed.";
                    }
                }
            }
            catch (Exception ex)
            {
                _Message.Status = "Failure";
                _Message.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                _Message.StatusMessage = ex.Message;
            }
            return Ok(_Message);
        }
    }
}
