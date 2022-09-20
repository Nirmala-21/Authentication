using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class GetAllCustomerStocksResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public double NetWorth { get; set; }
        public List<CustomerStockDetails> data { get; set; }
    }
    public class CustomerStockDetails
    {
        //ID, CustomerId, StocksId, StockName, StockPrice, StocksQuantity, TotalStockPrice
        public int ID { get; set; }
        public int CustomerId { get; set; }
        public int StocksId { get; set; }
        public string StockName { get; set; }
        public double StockPrice { get; set; }
        public int StocksQuantity { get; set; }
        public double TotalStockPrice { get; set; }
    }
}
