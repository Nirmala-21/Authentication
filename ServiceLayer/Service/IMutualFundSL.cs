using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public interface IMutualFundSL
    {
        public Task<AddMutualFundsResponse> AddMutualFunds(AddMutualFunds request);
        public Task<DeleteMutualFundsResponse> DeleteMutualFunds(DeleteMutualFundsRequest request);
        public Task<AddCustomerMutualFundsResponse> AddCustomerMutualFunds(AddCustomerMutualFunds request);
        public Task<GetAllMutualFundsResponse> GetAllMutualFunds();
        public Task<GetAllCustomerMutualFundsResponse> GetAllCustomerMutualFunds(int CustomerID);
    }
}
