using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class CustomerRegister
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailID { get; set; }
        public string Password { get; set; }
    }

    public class CustomerRegisterResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
