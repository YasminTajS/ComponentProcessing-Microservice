using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComponentProcessingService.Services
{
    public class Repair:IProcessingCharge
    {
        public double ProcessingTheService(bool isPriorityReq)
        {
            double cost = 500;
            if (isPriorityReq)
            {
                cost += 200;
                return cost;
            }
            else
            {
                return cost;
            }
        }
    }
}
