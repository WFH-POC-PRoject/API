﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnbDemoforPOC.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sql;

namespace AnbDemoforPOC.Controllerss
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
          
            var user = await _userManager.FindByNameAsync(objuserlogin.UserName);
            var role = await _userManager.GetRolesAsync(user);
            if (user != null && await _userManager.CheckPasswordAsync(user, objuserlogin.PasswordHash))
            {
                if (role != null && Convert.ToString(user.Managerid) != null)
                {
                    Console.WriteLine("Succefully Logged In!");
                }
                else
                {
                    Console.WriteLine("Role / Manager not be assigned.");
                }
            }
            else
            {
                Console.WriteLine("InCorrect Username or Password");
            }

            return Ok(user);
        }

    }

}

