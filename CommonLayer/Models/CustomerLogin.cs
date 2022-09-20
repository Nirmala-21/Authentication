using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class CustomerLogin
    {
        public string EmailID { get; set; }
        public string Password { get; set; }
    }

    public class CustomerLoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public CustomerData data { get; set; }
        public string Token { get; set; }
    }

    public class CustomerData
    {
        public string FullName { get; set; }
        public int customerId { get; set; }
    }

    public class ConnectivityResponse
    {
        public bool IsSuccess { get; set; }
    }
}
