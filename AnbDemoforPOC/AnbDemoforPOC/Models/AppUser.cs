using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AnbDemoforPOC.Models
{
    public class AppUser:IdentityUser<int>
    {
        //[Required]
        //public string UserName { get; set; }
        //[Required]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }

    }
}
