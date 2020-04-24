using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GetAllManagers.Entities;
using GetAllManagers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetAllManagers.Controllers
{
    [ApiController]
    [EnableCors("AllowMyOrign")]
    [Route("api/[controller]")]
    public class GetAllManagersController : ControllerBase
    {


        private readonly ILogger<GetAllManagersController> _logger;

        public GetAllManagersController(ILogger<GetAllManagersController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetAllManagers")]
        [AllowAnonymous]
        public IActionResult GetAllManagers()
        {
            Message _Message = new Message();

            EmployeeManagement_POCContext DBFactory = new EmployeeManagement_POCContext();

            try
            {
                var managesrs = DBFactory.AspNetUserRoles.Where(y => y.RoleId == 2).Select(x => x.UserId).Distinct().ToList().ToArray();
                if (managesrs == null || managesrs.Count() == 0)
                {
                    managesrs = DBFactory.AspNetUserRoles.Where(y => y.RoleId == 1).Select(x => x.UserId).Distinct().ToList().ToArray();

                }
                var managerinfo = DBFactory.AspNetUsers.Where(x => managesrs.Contains(x.Id)).Select(x => new { value = x.Id, label = x.FirstName + ' ' + x.LastName }).ToList();
                return Ok(managerinfo);
            }
            catch (Exception ex)
            {
                _Message.Status = "Failure";
                _Message.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                _Message.StatusMessage = ex.Message;
            }

            return Ok();
        }
    }
}
