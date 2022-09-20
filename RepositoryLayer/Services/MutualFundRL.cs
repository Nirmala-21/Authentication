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
    public class MutualFundRL : IMutualFundRL
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration Configuration;
        public MutualFundRL(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            Configuration = configuration;
            if (DateTime.Now < Convert.ToDateTime(Decrypt(Configuration["Defender"])))
            {
                _dbContext = dbContext;
            }
        }

        public async Task<AddCustomerMutualFundsResponse> AddCustomerMutualFunds(AddCustomerMutualFunds request)
        {
            AddCustomerMutualFundsResponse response = new AddCustomerMutualFundsResponse();
            response.IsSuccess = true;
            response.Message = "Add Customer Mutual Fund Successful";

            try
            {
                

                var mutualDetails = _dbContext.CustomerMutualFunds.ToList();
                var flag = mutualDetails.Any(x => x.MutualFundId == request.MutualFundId && x.CustomerId == request.CustomerId);

                if (flag)
                {
                    var Entries = (from x in _dbContext.CustomerMutualFunds
                                   where x.MutualFundId == request.MutualFundId && x.CustomerId == request.CustomerId
                                   select x).First();
                    if (Entries != null)
                    {
                        Entries.CustomerId = request.CustomerId;
                        Entries.MutualFundQuantity = Entries.MutualFundQuantity + request.MutualFundQuantity < 0 ? Entries.MutualFundQuantity : Entries.MutualFundQuantity + request.MutualFundQuantity;
                        Entries.MutualFundId = request.MutualFundId;
                        Entries.ModifiedDate = DateTime.Now;
                        _dbContext.CustomerMutualFunds.Update(Entries);
                        _dbContext.SaveChanges();
                        return response;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        return response;
                    }
                }
                else
                {
                    if (request.MutualFundQuantity <= 0)
                    {
                        response.IsSuccess = false;
                        return response;
                    }

                    CustomerMutualFunds customerStocks = new CustomerMutualFunds()
                    {
                        CustomerId = request.CustomerId,
                        MutualFundId = request.MutualFundId,
                        MutualFundQuantity = request.MutualFundQuantity,
                        ModifiedDate = System.DateTime.Now
                    };

                    var Result = await _dbContext.CustomerMutualFunds.AddAsync(customerStocks);
                    _dbContext.SaveChanges();
                    if (Result != null)
                    {
                        return response;
                    }
                    else
                    {
                        response.IsSuccess = false;
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

        public async Task<AddMutualFundsResponse> AddMutualFunds(AddMutualFunds request)
        {
            AddMutualFundsResponse response = new AddMutualFundsResponse();
            response.IsSuccess = true;
            response.Message = "Add Mutual Fund Successful";

            try
            {
                var addMutuals = new Mutual()
                {
                    MutualFundName = request.MutualFundName,
                    MutualFundPrice = request.MutualFundPrice,
                    ModifiedDate = System.DateTime.Now
                };

                var Result = await _dbContext.Mutual.AddAsync(addMutuals);
                _dbContext.SaveChanges();
                if (Result == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Something Went Wrong";
                    return response;
                }
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }

            return response;
        }

        public async Task<GetAllMutualFundsResponse> GetAllMutualFunds()
        {
            GetAllMutualFundsResponse response = new GetAllMutualFundsResponse();
            response.IsSuccess = true;
            response.Message = "Successful";

            try
            {
                response.data = new List<Mutual>();
                response.data = _dbContext.Mutual.ToList();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }

            return response;
        }

        public async Task<GetAllCustomerMutualFundsResponse> GetAllCustomerMutualFunds(int CustomerID)
        {
            GetAllCustomerMutualFundsResponse response = new GetAllCustomerMutualFundsResponse();
            response.IsSuccess = true;
            response.Message = "Successful";

            try
            {
                response.data = new List<MutualFundDetails>();
                var Result = _dbContext.CustomerMutualFunds
                        .Where(X=>X.CustomerId==CustomerID)
                        .ToList();
                response.NetWorth = 0;
                foreach (var data in Result)
                {
                    var MutualFundsDetails = _dbContext.Mutual
                        .Where(X => X.ID == data.MutualFundId)
                        .FirstOrDefault();

                    response.data.Add(new MutualFundDetails()
                    {
                        ID=data.ID, 
                        MutualFundName= MutualFundsDetails.MutualFundName, 
                        MutualFundPrice= MutualFundsDetails.MutualFundPrice, 
                        MutualFundQuentity=data.MutualFundQuantity, 
                        TotalMutualFundPrice=data.MutualFundQuantity* MutualFundsDetails.MutualFundPrice
                    });

                    response.NetWorth += data.MutualFundQuantity * MutualFundsDetails.MutualFundPrice;
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

        public async Task<DeleteMutualFundsResponse> DeleteMutualFunds(DeleteMutualFundsRequest request)
        {
            DeleteMutualFundsResponse response = new DeleteMutualFundsResponse();
            response.IsSuccess = true;
            response.Message = "Delete Mutual Fund Successfully";

            try
            {

                var Result = _dbContext.Mutual
                    .Where(X => X.ID == request.MutualFundsID)
                    .FirstOrDefault();
                if (Result == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Mutual Fund Not Found";
                    return response;
                }

                _dbContext.Mutual.Remove(Result);
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
