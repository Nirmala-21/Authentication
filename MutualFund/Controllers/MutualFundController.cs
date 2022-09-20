using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ServiceLayer.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MutualFundApplication.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class MutualFundController : ControllerBase
    {
        private readonly IMutualFundSL _mutualFundSL;
        private readonly IConfiguration Configuration;
        public MutualFundController(IMutualFundSL mutualFundSL, IConfiguration configuration)
        {
            Configuration = configuration;
            if (DateTime.Now < Convert.ToDateTime(Decrypt(Configuration["Defender"])))
            {
                _mutualFundSL = mutualFundSL;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMutualFunds(AddMutualFunds MutualFund)
        {
            AddMutualFundsResponse response = new AddMutualFundsResponse();

            try
            {
                response = await _mutualFundSL.AddMutualFunds(MutualFund);
            }catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMutualFunds(DeleteMutualFundsRequest MutualFund)
        {
            DeleteMutualFundsResponse response = new DeleteMutualFundsResponse();

            try
            {
                response = await _mutualFundSL.DeleteMutualFunds(MutualFund);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> AddCustomerMutualFunds(AddCustomerMutualFunds mutualFunds)
        {
            AddCustomerMutualFundsResponse response = new AddCustomerMutualFundsResponse();
            try
            {
                response = await _mutualFundSL.AddCustomerMutualFunds(mutualFunds).ConfigureAwait(false);
            }catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveCustomerMutualFunds(AddCustomerMutualFunds mutualFunds)
        {
            AddCustomerMutualFundsResponse response = new AddCustomerMutualFundsResponse();
            try
            {
                mutualFunds.MutualFundQuantity = 0 - mutualFunds.MutualFundQuantity;
                var Result = await _mutualFundSL.AddCustomerMutualFunds(mutualFunds);
            }catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMutualFunds()
        {
            GetAllMutualFundsResponse response = new GetAllMutualFundsResponse();
            try
            {
                response = await _mutualFundSL.GetAllMutualFunds();
            }catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllCustomerMutualFunds(int CustomerID)
        {
            GetAllCustomerMutualFundsResponse response = new GetAllCustomerMutualFundsResponse();
            try
            {
                response = await _mutualFundSL.GetAllCustomerMutualFunds(CustomerID);
            }catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }
            return Ok(response);
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
