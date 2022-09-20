using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class GetAllMutualFundsResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<Mutual> data { get; set; }
    }

    public class GetAllCustomerMutualFundsResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public double NetWorth { get; set; }
        public List<MutualFundDetails> data { get; set; }
    }

    public class MutualFundDetails
    {
        //ID, MutualFundName, MutualFundPrice, MutualFundQuentity, TotalMutualFundPrice
        public int ID { get; set; }
        public string MutualFundName { get; set; }
        public double MutualFundPrice { get; set; }
        public int MutualFundQuentity { get; set; }
        public double TotalMutualFundPrice { get; set; }
    }

}
