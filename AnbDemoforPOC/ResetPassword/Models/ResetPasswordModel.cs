using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResetPassword.Models
{
    public class ResetPasswordModel
    {
        public string Password { get; set; }
        public string Confirmpassword { get; set; }
        public string Id { get; set; }
        public string Token { get; set; }
    }
}
