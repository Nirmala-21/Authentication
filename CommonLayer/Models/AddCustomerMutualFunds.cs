using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class AddCustomerMutualFunds
    {
        public int CustomerId { get; set; }
        public int MutualFundId { get; set; }
        public int MutualFundQuantity { get; set; }
    }

    public class AddCustomerMutualFundsResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
