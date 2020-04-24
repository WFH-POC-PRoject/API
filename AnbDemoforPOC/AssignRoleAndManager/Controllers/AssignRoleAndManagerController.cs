using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AssignRoleAndManager.Entities;
using AssignRoleAndManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AssignRoleAndManager.Controllers
{
    
    [ApiController]
    [EnableCors("AllowMyOrign")]
    [Route("api/[controller]")]
    public class AssignRoleAndManagerController : ControllerBase
    {


        private readonly ILogger<AssignRoleAndManagerController> _logger;

        public AssignRoleAndManagerController(ILogger<AssignRoleAndManagerController> logger)
        {
            _logger = logger;
        }

        [HttpPost("AssignRoleAndManager")]
        [AllowAnonymous]
        public IActionResult AssignRoleAndManager(AssignRoleAndManagerModel assignRoleAndManagerModel)
        {
            Message _Message = new Message();
            EmployeeManagement_POCContext DBFactory = new EmployeeManagement_POCContext();

            try
            {
                if (assignRoleAndManagerModel.UserId > 0)
                {
                    var olddata = DBFactory.AspNetUsers.Where(x => x.Id == Convert.ToInt32(assignRoleAndManagerModel.UserId)).FirstOrDefault();
                    if (olddata != null)
                    {
                        olddata.Managerid = Convert.ToInt32(assignRoleAndManagerModel.Managerid);
                        DBFactory.Entry(olddata).State = EntityState.Modified;
                        DBFactory.SaveChanges();

                    }
                }
                if (assignRoleAndManagerModel.RoleId > 0)
                {
                    var addRole = DBFactory.AspNetUserRoles.Where(x => x.UserId == Convert.ToInt32(assignRoleAndManagerModel.UserId)).FirstOrDefault();
                    if (addRole == null)
                    {
                        AspNetUserRoles _aspNetUserRoles = new AspNetUserRoles();
                        _aspNetUserRoles.RoleId = Convert.ToInt32(assignRoleAndManagerModel.RoleId);
                        _aspNetUserRoles.UserId = Convert.ToInt32(assignRoleAndManagerModel.UserId);

                        DBFactory.AspNetUserRoles.Add(_aspNetUserRoles);
                        var flag = DBFactory.SaveChanges();
                        if (flag > 0)
                        {
                            _Message.Status = "Success";
                            _Message.StatusCode = 200;
                            _Message.StatusMessage = "Role Successfully Asigned.";
                        }
                        return Ok(_Message);
                    }
                    else
                    {
                        var existRole = DBFactory.AspNetUserRoles.Where(x => x.UserId == Convert.ToInt32(assignRoleAndManagerModel.UserId)).FirstOrDefault();
                        DBFactory.AspNetUserRoles.Remove(existRole);
                        DBFactory.SaveChanges();
                        
                        AspNetUserRoles _aspNetUserRoles = new AspNetUserRoles();
                        _aspNetUserRoles.RoleId = Convert.ToInt32(assignRoleAndManagerModel.RoleId);
                        _aspNetUserRoles.UserId = Convert.ToInt32(assignRoleAndManagerModel.UserId);
                        DBFactory.AspNetUserRoles.Add(_aspNetUserRoles);
                        var flag = DBFactory.SaveChanges();
                        if (flag > 0)
                        {
                            _Message.Status = "Success";
                            _Message.StatusCode = 200;
                            _Message.StatusMessage = "Role Successfully updated.";
                        }
                        return Ok(_Message);
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
