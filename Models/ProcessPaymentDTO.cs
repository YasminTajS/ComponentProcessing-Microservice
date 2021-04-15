using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComponentProcessingService.Models
{
    public class ProcessPaymentDTO
    {
        public int RequestId { get; set; }
        public string CardNumber { get; set; }
        public double CardLimit { get; set; }
        public double ProcessCharge { get; set; }
    }
}
