using CommonLayer.Models;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public class AuthenticationSL : IAuthenticationSL
    {
        public readonly IAuthenticationRL _authenticationRL;
        public AuthenticationSL(IAuthenticationRL authenticationRL)
        {
            _authenticationRL = authenticationRL;
        }

        public async Task<CustomerLoginResponse> Login(CustomerLogin request)
        {
            return await _authenticationRL.Login(request);
        }

        public async Task<CustomerRegisterResponse> RegisterUser(CustomerRegister register)
        {
            return await _authenticationRL.RegisterUser(register);
        }
    }
}
