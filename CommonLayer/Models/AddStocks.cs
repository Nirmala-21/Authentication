using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class AddStocks
    {
        public string StockName { get; set; }
        public double StockPrice { get; set; }
    }

    public class AddStocksResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
