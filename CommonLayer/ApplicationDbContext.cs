using CommonLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public Microsoft.EntityFrameworkCore.DbSet<CustomerDetail> Customer { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<Mutual> Mutual { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<CustomerMutualFunds> CustomerMutualFunds { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<CustomerStocks> CustomerStocks { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<Stocks> Stocks { get; set; }

    }
}
