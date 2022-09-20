using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class GetAllPortFolioDetailsResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int PortfolioId { get; set; }
        public List<StocksResponse> StockDetails { get; set; }
        public List<MutualResponse> MutualFundsDetails { get; set; }
    }

    public class StocksResponse
    {
        public string StockName { get; set; }
        public double StockPrice { get; set; }
        public int StockQuantity { get; set; }
        public double TotalStockPrice { get; set; }

    }

    public class MutualResponse
    {
        public string MutualFundName { get; set; }
        public double MutualFundPrice { get; set; }
        public int Quantity { get; set; }
        public double TotalMutualFundPrice { get; set; }
    }
}
