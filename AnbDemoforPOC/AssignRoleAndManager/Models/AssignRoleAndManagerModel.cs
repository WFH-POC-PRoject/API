using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssignRoleAndManager.Models
{
    public class AssignRoleAndManagerModel
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int Managerid { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

    }
}
