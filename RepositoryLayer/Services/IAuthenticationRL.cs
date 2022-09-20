using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public interface IAuthenticationRL
    {
        public Task<CustomerRegisterResponse> RegisterUser(CustomerRegister request);
        public Task<CustomerLoginResponse> Login(CustomerLogin request);
    }
}
