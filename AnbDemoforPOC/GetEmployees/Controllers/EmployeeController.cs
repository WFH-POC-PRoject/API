using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Employee.Entities;
using Employee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Employee.Controllers
{
    [ApiController]
    [EnableCors("AllowMyOrign")]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        //private IdentityUserRole<AppUser> userRolemgr;
        private RoleManager<AppRole> roleMgr;
        private UserManager<AppUser> userMgr;
        private SignInManager<AppUser> siginMgr;
        public EmployeeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            userMgr = userManager;
            roleMgr = roleManager;
            siginMgr = signInManager;
        }

        [HttpGet("GetAllEmployees")]
        [AllowAnonymous]
        public IActionResult GetAllEmployees(string value)
        {
            List<UserRoles> Alldata = new List<UserRoles>();
            List<UserRoles> Alldata1 = new List<UserRoles>();
            List<UserRoles> Alldata2 = new List<UserRoles>();
            Message _Message = new Message();
            EmployeeManagement_POCContext DBFactory = new EmployeeManagement_POCContext();

            try
            {
                if (value != null)
                {
                    var alldata = (from User in DBFactory.AspNetUsers

                                   join WU in DBFactory.AspNetUserRoles on User.Id equals WU.UserId
                                   join WR in DBFactory.AspNetRoles on WU.RoleId equals WR.Id
                                   where (User.Id == Convert.ToInt32(value))
                                   select new UserRoles
                                   {
                                       UserId = User.Id,
                                       UserName = User.UserName,
                                       FirstName = User.FirstName,
                                       LastName = User.FirstName,
                                       Email = User.Email,
                                       RoleName = WR.Name,
                                       RoleId = WR.Id,
                                       ManagerName = DBFactory.AspNetUsers.Where(x =>x.Id==User.Managerid).Select(x=>x.UserName).FirstOrDefault(),
                                       ManagerId = User.Managerid
                                   }).FirstOrDefault();
                    Alldata.Add(alldata);
                }
                else
                {
                     Alldata1 = (from user in DBFactory.AspNetUsers
                                where !DBFactory.AspNetUserRoles.Any(c => user.Id == c.UserId)
                                select new UserRoles
                               {
                                   UserId = user.Id,
                                   UserName = user.UserName,
                                   FirstName = user.FirstName,
                                   LastName = user.FirstName,
                                   Email = user.Email,
                                   RoleName = "",
                                   RoleId = 0,
                                    ManagerName = DBFactory.AspNetUsers.Where(x => x.Id == user.Managerid).Select(x => x.UserName).FirstOrDefault(),
                                    ManagerId = user.Managerid
                                }).ToList();


                    
                    Alldata2 = (from User in DBFactory.AspNetUsers
                               join WU in DBFactory.AspNetUserRoles on User.Id equals WU.UserId
                               join WR in DBFactory.AspNetRoles on WU.RoleId equals WR.Id
                               select new UserRoles
                               {
                                   UserId = User.Id,
                                   UserName = User.UserName,
                                   FirstName = User.FirstName,
                                   LastName = User.FirstName,
                                   Email = User.Email,
                                   RoleName = WR.Name,
                                   RoleId = WR.Id,
                                   ManagerName = DBFactory.AspNetUsers.Where(x => x.Id == User.Managerid).Select(x => x.UserName).FirstOrDefault(),
                                   ManagerId = User.Managerid
                               }).ToList();



                    Alldata.AddRange(Alldata1);
                    Alldata.AddRange(Alldata2);
                    Alldata.OrderBy(x => x.UserId).ToList();


                }
            }
            catch (Exception ex)
            {
                _Message.Status = "Failure";
                _Message.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                _Message.StatusMessage = ex.Message;
            }

            return Ok(Alldata);
        }



    }
}
