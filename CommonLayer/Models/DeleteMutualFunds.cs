using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class DeleteMutualFundsRequest
    {
        [Required]
        public int MutualFundsID { get; set; }
    }

    public class DeleteMutualFundsResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
