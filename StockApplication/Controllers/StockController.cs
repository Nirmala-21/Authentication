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

namespace StockApplication.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockSL _stockSL;
        private readonly IConfiguration Configuration;
        public StockController(IStockSL stockSL, IConfiguration configuration)
        {
            Configuration = configuration;
            if (DateTime.Now < Convert.ToDateTime(Decrypt(Configuration["Defender"])))
            {
                _stockSL = stockSL;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddStocks(AddStocks request)
        {
            AddStocksResponse response = new AddStocksResponse();
            try
            {
                response = await _stockSL.AddStocks(request);

            }catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }
            return Ok(response);
        }

        //
        [HttpGet]
        public async Task<IActionResult> GetAllStocks()
        {
            GetAllStocks response = new GetAllStocks();

            try
            {
                response = await _stockSL.GetAllStocks();

            }catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStocks(DeleteStocksRequest request)
        {
            DeleteStocksResponse response = new DeleteStocksResponse();
            try
            {
                response = await _stockSL.DeleteStocks(request);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> AddCustomerStocks(AddCustomerStocksRequest request)
        {
            AddCustomerStocksResponse response = new AddCustomerStocksResponse();
            try
            {

                response = await _stockSL.AddCustomerStocks(request);

            }catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllCustomerStocks(int CustomerId)
        {
            GetAllCustomerStocksResponse response = new GetAllCustomerStocksResponse();
            try
            {
                 response = await _stockSL.GetAllCustomerStocks(CustomerId);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveCustomerStocks(AddCustomerStocksRequest stocks)
        {
            AddCustomerStocksResponse response = new AddCustomerStocksResponse();
            try
            {
                stocks.StocksQuantity = 0 - stocks.StocksQuantity;
                response = await _stockSL.AddCustomerStocks(stocks);

            }catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        public IActionResult GetAllPortFolioDetails(int CustomerId)
        {
            GetAllPortFolioDetailsResponse response = new GetAllPortFolioDetailsResponse();

            try
            {
                //response = await _stockSL.GetAllPortFolioDetails(CustomerId);

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
