using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UpdateEmployee.Models;

namespace UpdateEmployee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class UpdateEmployeeController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public UpdateEmployeeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(User user)
        {
            Message _Message = new Message();
            try
            {
                AppUser users = await _userManager.FindByIdAsync(Convert.ToString(user.Id));

                if (users != null)
                {
                    AppUser userEmail = await _userManager.FindByEmailAsync(user.Email);
                    AppUser checkwithusername = await _userManager.FindByNameAsync(user.UserName);
                    if (users.UserName != user.UserName && checkwithusername!=null)
                    {
                        _Message.Status = "Failure";
                        _Message.StatusCode = 102;
                        _Message.StatusMessage = "User already existed with this User Name.";
                    }
                    else if (users.Email != user.Email && userEmail != null)
                    {
                        _Message.Status = "Failure";
                        _Message.StatusCode = 101;
                        _Message.StatusMessage = "User already existed with this Email Id.";
                    }
                    else
                    {
                        users.UserName = (user.UserName != null || user.UserName != "") ? user.UserName : users.UserName;
                        users.FirstName = (user.FirstName != null || user.FirstName != "") ? user.FirstName : users.FirstName;
                        users.LastName = (user.LastName != null || user.LastName != "") ? user.LastName : users.LastName;
                        users.Email = (user.Email != null || user.Email != "") ? user.Email : users.Email;

                        IdentityResult result = await _userManager.UpdateAsync(users);
                        if (result.Succeeded == true)
                        {
                            _Message.Status = "Success";
                            _Message.StatusCode = 200;
                            _Message.StatusMessage = "User Succefully Updated !!";
                        }
                        else
                        {
                            _Message.Status = "Failure";
                            _Message.StatusCode = 103;
                            _Message.StatusMessage = "User not updated !!";
                        }
                    }
                     
                }
                else
                {
                    _Message.Status = "Failure";
                    _Message.StatusCode = 104;
                    _Message.StatusMessage = "User does not exist !!";
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