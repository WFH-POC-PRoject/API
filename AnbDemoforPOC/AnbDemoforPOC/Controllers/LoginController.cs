﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AnbDemoforPOC.Models;
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
            AppUser appUser = new AppUser();
            var user = await _userManager.FindByNameAsync(objuserlogin.UserName);
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);

                if (role.Count>0)
                {
                    if (await _userManager.CheckPasswordAsync(user, objuserlogin.PasswordHash))
                    {
                        appUser.Id = 1;
                        appUser.UserName = user.UserName;
                        appUser.NormalizedUserName = role.FirstOrDefault();
                        return Ok(appUser);
                    }
                    else
                    {
                        appUser.Id = 2;
                        appUser.UserName = "";
                        appUser.NormalizedUserName = "";
                        return Ok(appUser);
                    }
                }
                else
                {
                    appUser.Id = 3;
                    appUser.UserName = "";
                    appUser.NormalizedUserName = "";
                    return Ok(appUser);
                }
            }
            else
            {
                appUser.Id = 4;
                appUser.UserName = "";
                appUser.NormalizedUserName = "";
                return Ok(appUser);
            }
        }

    }

}

