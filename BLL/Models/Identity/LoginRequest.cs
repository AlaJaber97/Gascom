using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsRememberMe { get; set; }
    }
}
