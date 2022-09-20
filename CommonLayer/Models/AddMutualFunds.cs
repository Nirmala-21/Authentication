using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class AddMutualFunds
    {
        public string MutualFundName { get; set; }
        public double MutualFundPrice { get; set; }
    }

    public class AddMutualFundsResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
