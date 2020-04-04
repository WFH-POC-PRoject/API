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
                if (user == null)
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
                        // string callbackUrl = "file://C:/Users/sriram.biccavolu/Desktop/resetform.html";
                        //string callbackUrl = "https://localhost:44325/Home/Privacy?IsSuperUser=" + appUser.Email;
                        string configcallbackUrl = _configuration.GetValue<string>("callbackUrl");
                        string callbackUrl = configcallbackUrl + user.Id;
                        var user1 = await userMgr.FindByEmailAsync(appUser.Email);
                        //var code = await userMgr.GeneratePasswordResetTokenAsync(user1);
                        var subject = "Reset password request.";
                        var body = "<html><body>" +
                                   $"<h3>Hi {user1.FirstName.ToUpper().ToString()}!</h3>" +
                                   $"<h3>Your account has been created , please click below link to reset your password</h3>" +
                                   $"<a  href=\"" + callbackUrl + "\"> Reset password </a>" +
                                   $"<h4>Thanks</h4>"+
                        "</body></html>";
                        var message = new IdentityMessage
                        {
                            Body = body,
                            Destination = appUser.Email,
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
                        _Message.StatusMessage = "User Successfully Created !!";
                    }
                    else
                    {
                        _Message.Status = "Failure";
                        _Message.StatusCode = 200;
                        _Message.StatusMessage = "User Already Existed !!";
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
