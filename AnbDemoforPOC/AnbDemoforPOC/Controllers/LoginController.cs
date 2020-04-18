using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AnbDemoforPOC.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sql;

namespace AnbDemoforPOC.Controllerss
{
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    [System.Web.Http.Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [System.Web.Http.HttpGet]
        public IEnumerable<AppUser> Login()
        {
            AppUser x = new AppUser();
            return Enumerable.Range(1, 1).Select(index => new AppUser
            {
                UserName = x.UserName,
                PasswordHash = x.PasswordHash,
            }).ToArray();
        }

        [System.Web.Http.HttpPost]
        public async Task<IActionResult> Login(AppUser objuserlogin)
        {
            AppUser appUser = new AppUser();
            var user = await _userManager.FindByNameAsync(objuserlogin.UserName);
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);

                if (role != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, objuserlogin.PasswordHash))
                    {
                        appUser.Id = 1;
                        appUser.UserName = role.FirstOrDefault();
                        return Ok(appUser);
                    }
                    else
                    {
                        appUser.Id = 2;
                        appUser.UserName = "";
                        return Ok(appUser);
                    }
                }
                else
                {
                    appUser.Id = 3;
                    appUser.UserName = "";
                    return Ok(appUser);
                }
            }
            else
            {
                appUser.Id = 4;
                appUser.UserName = "";
                return Ok(appUser);
            }
        }

    }

}

