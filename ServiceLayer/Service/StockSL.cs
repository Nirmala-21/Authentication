using CommonLayer.Models;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public class StockSL : IStockSL
    {
        private readonly IStockRL _stockRL;
        public StockSL(IStockRL stockRL)
        {
            _stockRL = stockRL;
        }

        public async Task<AddCustomerStocksResponse> AddCustomerStocks(AddCustomerStocksRequest request)
        {
            return await _stockRL.AddCustomerStocks(request);
        }

        public async Task<AddStocksResponse> AddStocks(AddStocks request)
        {
            return await _stockRL.AddStocks(request); 
        }

        public async Task<DeleteStocksResponse> DeleteStocks(DeleteStocksRequest request)
        {
            return await _stockRL.DeleteStocks(request);
        }

        public async Task<GetAllCustomerStocksResponse> GetAllCustomerStocks(int CustomerID)
        {
            return await _stockRL.GetAllCustomerStocks(CustomerID);
        }

        public async Task<GetAllPortFolioDetailsResponse> GetAllPortFolioDetails(int CustomerID)
        {
            return await _stockRL.GetAllPortFolioDetails(CustomerID);
        }

        public async Task<GetAllStocks> GetAllStocks()
        {
            return await _stockRL.GetAllStocks();
        }
    }
}
