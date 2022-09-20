using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
   public class AddCustomerStocksRequest
    {
        public int CustomerId { get; set; }
        public int StocksId { get; set; }
        public int StocksQuantity { get; set; }
    }

    public class AddCustomerStocksResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
