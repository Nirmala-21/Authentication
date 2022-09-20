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
    public class StockRL : IStockRL
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration Configuration;

        public StockRL(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            Configuration = configuration;
            if (DateTime.Now < Convert.ToDateTime(Decrypt(Configuration["Defender"])))
            {
                _dbContext = dbContext;
            }
        }
        public async Task<AddStocksResponse> AddStocks(AddStocks request)
        {
            AddStocksResponse response = new AddStocksResponse();
            response.IsSuccess = true;
            response.Message = "Add Stocks Successfully";

            try
            {

                Stocks stocks = new Stocks()
                {
                    StockName = request.StockName,
                    StockPrice = request.StockPrice,
                    CreateDate = DateTime.Now
                };

                await _dbContext.AddAsync(stocks);
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

        public async Task<GetAllStocks> GetAllStocks()
        {
            GetAllStocks response = new GetAllStocks();
            response.IsSuccess = true;
            response.Message = "Successfully";

            try
            {

                response.data = new List<Stocks>();
                response.data = _dbContext.Stocks.ToList();

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }

            return response;
        }

        public async Task<AddCustomerStocksResponse> AddCustomerStocks(AddCustomerStocksRequest request)
        {
            AddCustomerStocksResponse response = new AddCustomerStocksResponse();
            response.IsSuccess = true;
            response.Message = "Add Customer Stock Successfully";

            try
            {
                var stockDetails = _dbContext.CustomerStocks.ToList();
                var flag = stockDetails.Any(x => x.StocksId == request.StocksId && x.CustomerId == request.CustomerId);

                if (flag)
                {
                    var Entries = (from x in _dbContext.CustomerStocks
                                   where x.StocksId == request.StocksId && x.CustomerId == request.CustomerId
                                   select x).First();
                    if (Entries != null)
                    {
                        Entries.StocksId = request.StocksId;
                        Entries.StocksQuantity = Entries.StocksQuantity + request.StocksQuantity < 0 ? Entries.StocksQuantity : Entries.StocksQuantity + request.StocksQuantity;
                        Entries.CustomerId = request.CustomerId;
                        Entries.ModifiedDate = DateTime.Now;
                        _dbContext.CustomerStocks.Update(Entries);
                        _dbContext.SaveChanges();
                        return response;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Something went wrong";
                        return response;
                    }
                }
                else
                {
                    if (request.StocksQuantity <= 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "Something went wrong";
                        return response;
                    }

                    CustomerStocks stocks = new CustomerStocks()
                    {
                        CreateDate = DateTime.Now,
                        CustomerId = request.CustomerId,
                        StocksId = request.StocksId,
                        StocksQuantity = request.StocksQuantity
                    };

                    var Result = await _dbContext.CustomerStocks.AddAsync(stocks);
                    _dbContext.SaveChanges();
                    if (Result != null)
                    {
                        return response;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Something went wrong";
                        return response;
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }

            return response;
        }

        public async Task<GetAllCustomerStocksResponse> GetAllCustomerStocks(int CustomerID)
        {
            GetAllCustomerStocksResponse response = new GetAllCustomerStocksResponse();
            response.IsSuccess = true;
            response.Message = "Successfully";

            try
            {
                response.data = new List<CustomerStockDetails>();
                var Result = _dbContext.CustomerStocks.Where(x => x.CustomerId == CustomerID).ToList();
                response.NetWorth = 0;
                foreach(var data in Result)
                {
                    var StockDetails = _dbContext.Stocks.Where(X => X.ID == data.StocksId).FirstOrDefault();
                    response.data.Add(new CustomerStockDetails()
                    {
                        ID=data.ID, 
                        CustomerId=data.CustomerId, 
                        StocksId=data.StocksId, 
                        StockName=StockDetails.StockName, 
                        StockPrice= StockDetails.StockPrice, 
                        StocksQuantity= data.StocksQuantity, 
                        TotalStockPrice= data.StocksQuantity* StockDetails.StockPrice,
                    });

                    response.NetWorth += data.StocksQuantity * StockDetails.StockPrice;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }

            return response;
        }

        public async Task<GetAllPortFolioDetailsResponse> GetAllPortFolioDetails(int CustomerId)
        {
            GetAllPortFolioDetailsResponse response = new GetAllPortFolioDetailsResponse();
            response.IsSuccess = true;
            response.Message = "Successful";

            try
            {

                List<StocksResponse> ListStockDetails = new List<StocksResponse>();
                List<MutualResponse> ListMutualFundsDetails = new List<MutualResponse>();
                var CustomerStockDetails = _dbContext.CustomerStocks.Where(x => x.CustomerId == CustomerId).ToList();
                var CustomerMutualDetails = _dbContext.CustomerMutualFunds.Where(x => x.CustomerId == CustomerId).ToList();
                var stocksDetails = _dbContext.Stocks.ToList();
                var mutualFundDetails = _dbContext.Mutual.ToList();
                foreach (var item in CustomerStockDetails)
                {
                    var result = stocksDetails.Where(x => x.ID == item.StocksId).FirstOrDefault();
                    ListStockDetails.Add(new StocksResponse()
                    {
                        StockName = result.StockName,
                        StockPrice = result.StockPrice,
                        StockQuantity = item.StocksQuantity,
                        TotalStockPrice = result.StockPrice * item.StocksQuantity
                    });
                }

                foreach (var item in CustomerMutualDetails)
                {
                    var result = mutualFundDetails.Where(x => x.ID == item.MutualFundId).FirstOrDefault();
                    ListMutualFundsDetails.Add(new MutualResponse()
                    {
                        MutualFundName = result.MutualFundName,
                        MutualFundPrice = result.MutualFundPrice,
                        Quantity = item.MutualFundQuantity,
                        TotalMutualFundPrice = result.MutualFundPrice * item.MutualFundQuantity
                    });
                }

                response.StockDetails = ListStockDetails;
                response.MutualFundsDetails = ListMutualFundsDetails;
                response.PortfolioId = CustomerId;

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

        public async Task<DeleteStocksResponse> DeleteStocks(DeleteStocksRequest request)
        {
            DeleteStocksResponse response = new DeleteStocksResponse();
            response.IsSuccess = true;
            response.Message = "Delete Stock Successfully";

            try
            {

                var Result = _dbContext.Stocks
                    .Where(X=>X.ID==request.StockID)
                    .FirstOrDefault();
                if (Result == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Stock Not Found";
                    return response;
                }

                _dbContext.Stocks.Remove(Result);
                int DeleteResult = await _dbContext.SaveChangesAsync();
                if (DeleteResult <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Something went Wrong";
                }


            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }

            return response;
        }
    }
}
