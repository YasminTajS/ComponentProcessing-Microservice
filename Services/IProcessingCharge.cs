using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComponentProcessingService.Services
{
    public interface IProcessingCharge
    {
        public double ProcessingTheService(bool isPriorityReq);
    }
}
