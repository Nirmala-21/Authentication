using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class CustomerDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailID { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }

    public class Mutual
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string MutualFundName { get; set; }
        public double MutualFundPrice { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; }
    }

    public class CustomerMutualFunds
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CustomerId { get; set; }
        public int MutualFundId { get; set; }
        public int MutualFundQuantity { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; }
    }

    public class CustomerStocks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CustomerId { get; set; }
        public int StocksId { get; set; }
        public int StocksQuantity { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; }
    }

    public class Stocks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string StockName { get; set; }
        public double StockPrice { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; }
    }
}
