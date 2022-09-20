using CommonLayer.Models;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public class MutualFundSL : IMutualFundSL
    {
        private readonly IMutualFundRL _mutualFundRL;
        public MutualFundSL(IMutualFundRL mutualFundRL)
        {
            _mutualFundRL = mutualFundRL;
        }

        public async Task<AddCustomerMutualFundsResponse> AddCustomerMutualFunds(AddCustomerMutualFunds request)
        {
            return await _mutualFundRL.AddCustomerMutualFunds(request);
        }

        public async Task<AddMutualFundsResponse> AddMutualFunds(AddMutualFunds request)
        {
            return await _mutualFundRL.AddMutualFunds(request);
        }

        public async Task<DeleteMutualFundsResponse> DeleteMutualFunds(DeleteMutualFundsRequest request)
        {
            return await _mutualFundRL.DeleteMutualFunds(request);
        }

        public async Task<GetAllCustomerMutualFundsResponse> GetAllCustomerMutualFunds(int CustomerID)
        {
            return await _mutualFundRL.GetAllCustomerMutualFunds(CustomerID);
        }

        public async Task<GetAllMutualFundsResponse> GetAllMutualFunds()
        {
            return await _mutualFundRL.GetAllMutualFunds();
        }
    }
}
