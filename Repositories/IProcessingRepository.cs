using ComponentProcessingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComponentProcessingService.Repositories
{
    public interface IProcessingRepository
    {
        public IEnumerable<ProcessRequest> Get();
        public int GenerateId();

        public void AddRequest(ProcessRequest processRequest);

        public void AddResponse(ProcessResponse processResponse);
        public void UpdataBalance(string cardNumber, double newLimit);
        public double GetLimit(string cardNumber);

    }
}
