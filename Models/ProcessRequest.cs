using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComponentProcessingService.Models
{
    public class ProcessRequest
    {
        public string Name { get; set; }
        public string ConactNumber { get; set; }
        public string CreditCardNumber { get; set; }
        public string ComponentType { get; set; }
        public string ComponentName { get; set; }
        public int Qunatity { get; set; }
        public bool IsPriorityRequest { get; set; }

    }
}
