using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AnbDemoforPOC.Models;
using Login.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AnbDemoforPOC.Controllers
{
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IEnumerable<AppUser> Login()
        {
            AppUser x = new AppUser();
            return Enumerable.Range(1, 1).Select(index => new AppUser
            {
                UserName = x.UserName,
                PasswordHash = x.PasswordHash,
            }).ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AppUser objuserlogin)
        {
            ManageUser manageUser = new ManageUser();
            AppUser appUser = new AppUser();
            var user = await _userManager.FindByNameAsync(objuserlogin.UserName);
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);

                if (role.Count>0)
                {
                    if (await _userManager.CheckPasswordAsync(user, objuserlogin.PasswordHash))
                    {

                        manageUser.StatusCode = 1;
                        manageUser.UserName = user.UserName;
                        manageUser.UserRole = role.FirstOrDefault();
                        manageUser.UserId = Convert.ToString(user.Id);
                        return Ok(manageUser);
                    }
                    else
                    {
                        manageUser.StatusCode = 2;
                        manageUser.UserName = "";
                        manageUser.UserRole = "";
                        manageUser.UserId = "";
                        return Ok(manageUser);
                    }
                }
                else
                {
                    manageUser.StatusCode = 3;
                    manageUser.UserName = "";
                    manageUser.UserRole = "";
                    manageUser.UserId = "";
                    return Ok(manageUser);
                }
            }
            else
            {
                manageUser.StatusCode = 4;
                manageUser.UserName = "";
                manageUser.UserRole = "";
                manageUser.UserId = "";
                return Ok(manageUser);
            }
        }

    }

}

