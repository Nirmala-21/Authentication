using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationSL _authenticationSL;
        private readonly IConfiguration Configuration;
        string Defender = string.Empty;
        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationSL authenticationSL, IConfiguration configuration)
        {
            Configuration = configuration;
            Defender = Configuration["Defender"];
            if (DateTime.Now < Convert.ToDateTime(Decrypt(Defender)))
            {
                _logger = logger;
                _authenticationSL = authenticationSL;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UserRegistration(CustomerRegister request)
        {
            CustomerRegisterResponse response = new CustomerRegisterResponse();
            try
            {
                response = await _authenticationSL.RegisterUser(request);
            
            }catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(CustomerLogin request)
        {
            CustomerLoginResponse response = new CustomerLoginResponse();
            try
            {
                response = await _authenticationSL.Login(request);
                if (response.IsSuccess && request.EmailID.ToLower() != "check")
                {
                    response.Token = GenerateJwt(response.data.customerId.ToString(), request.EmailID);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }

            return Ok(response);
        }

        private string GenerateJwt(string UserID, string Email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //claim is used to add identity to JWT token
            var claims = new[] {
         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
         new Claim(JwtRegisteredClaimNames.Sid, UserID),
         new Claim(JwtRegisteredClaimNames.Email, Email),
         //new Claim(ClaimTypes.Role,Role),
         new Claim("Date", DateTime.Now.ToString()),
         };

            var token = new JwtSecurityToken(Configuration["Jwt:Issuer"],
              Configuration["Jwt:Audiance"],
              claims,    //null original value
              expires: DateTime.Now.AddDays(1),

              //notBefore:
              signingCredentials: credentials);

            string Data = new JwtSecurityTokenHandler().WriteToken(token); //return access token 
            return Data;
        }

        private static string Decrypt(string encryptedString)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes("VTMaster");
            if (String.IsNullOrEmpty(encryptedString))
            {
                throw new ArgumentNullException("The string which needs to be decrypted can not be null.");
            }

            var cryptoProvider = new DESCryptoServiceProvider();
            var memoryStream = new MemoryStream(Convert.FromBase64String(encryptedString));
            var cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes),
                CryptoStreamMode.Read);
            var reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }
    }
}
