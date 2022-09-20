using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public interface IStockRL
    {
        public Task<AddStocksResponse> AddStocks(AddStocks request);
        public Task<GetAllStocks> GetAllStocks();
        public Task<DeleteStocksResponse> DeleteStocks(DeleteStocksRequest request);
        public Task<AddCustomerStocksResponse> AddCustomerStocks(AddCustomerStocksRequest request);
        public Task<GetAllCustomerStocksResponse> GetAllCustomerStocks(int CustomerID);
        public Task<GetAllPortFolioDetailsResponse> GetAllPortFolioDetails(int CustomerID);

    }
}
