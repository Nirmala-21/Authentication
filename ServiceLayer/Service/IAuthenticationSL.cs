using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public interface IAuthenticationSL
    {
        public Task<CustomerRegisterResponse> RegisterUser(CustomerRegister User);
        public Task<CustomerLoginResponse> Login(CustomerLogin request);

    }
}
