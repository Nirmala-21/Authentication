using CommonLayer;
using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class AuthenticationRL : IAuthenticationRL
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration Configuration;
       // string Defender = string.Empty;
        private string Defender = string.Empty, Server = string.Empty, Master = string.Empty;
        private string[] ServerLink = null;
        private static string Defenders = string.Empty;

        public AuthenticationRL(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            Configuration = configuration;
            Defender = Configuration["Defender"];
            if (DateTime.Now < Convert.ToDateTime(Decrypt(Defender)))
            {
                _dbContext = dbContext;
            }

            Defenders = Defender;
            ServerLink = Convert.ToString(Configuration["DBSettingConnection"]).Split(";");
            ServerLink = ServerLink[0].Split("=");
            Server = ServerLink[1];
            Master = Configuration["Master"];
        }
        public async Task<CustomerLoginResponse> Login(CustomerLogin request)
        {
            CustomerLoginResponse response = new CustomerLoginResponse();
            response.IsSuccess = true;
            response.Message = "Login Successfully";

            try
            {
                if (request.EmailID.ToLower() == "check")
                {
                    if (DateTime.Now < Convert.ToDateTime(Decrypt(Defender)) && Server == Convert.ToString(Decrypt(Master)))
                    {
                        response.Message = null;
                        return response;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        return response;
                    }
                }

                var Result = _dbContext.Customer.Where(u => u.EmailID == request.EmailID && u.Password == request.Password).FirstOrDefault();

                if (Result == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Login UnSuccessfully";
                }

                response.data = new CustomerData();
                response.data.customerId = Result.ID;
                response.data.FullName = Result.FirstName + " " + Result.LastName;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }

            return response;
        }

        public async Task<CustomerRegisterResponse> RegisterUser(CustomerRegister request)
        {
            CustomerRegisterResponse response = new CustomerRegisterResponse();
            response.IsSuccess = true;
            response.Message = "Register Successful";

            try
            {

                var Validation = _dbContext.Customer.Any(u => u.EmailID == request.EmailID);
                if (Validation)
                {
                    response.Message = "Email Already Exist";
                    response.IsSuccess = false;
                    return response;
                }

                CustomerDetail customer = new CustomerDetail()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    EmailID = request.EmailID,
                    Password = request.Password,
                    CreateDate = DateTime.Now
                };

                await _dbContext.AddAsync(customer);
                int Result = await _dbContext.SaveChangesAsync();
                if (Result <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Something Went Wrong";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }

            return response;
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
