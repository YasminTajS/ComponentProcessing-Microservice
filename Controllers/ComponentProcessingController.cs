using ComponentProcessingService.Models;
using ComponentProcessingService.Repositories;
using ComponentProcessingService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ComponentProcessingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ComponentProcessingController : ControllerBase
    {
        private readonly IProcessingRepository _repo;
        public ComponentProcessingController(IProcessingRepository repository)
        {
            _repo = repository;
        }
        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<ProcessRequest>> GetAll()
        {
            var temp = _repo.Get();
            return Ok(temp);
        }
        [HttpPost("ProcessDetails")]
        public ActionResult<ProcessResponse> ProcessDetails(ProcessRequest processRequest)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress =new Uri("http://localhost:51312");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                HttpResponseMessage response = client.GetAsync("api/PackagingAndDelivery?item=" + processRequest.ComponentType + "&count=" + processRequest.Qunatity).Result;
                if (response.IsSuccessStatusCode)
                {
                    double cost = response.Content.ReadAsAsync<double>().Result;
                    ProcessResponse response1 = new ProcessResponse();
                    IProcessingCharge processingCharge;
                    response1.RequestId = _repo.GenerateId();
                    response1.PackagingAndDeliveryCharge = cost;
                    response1.DateOfDelivery = DateTime.Now.Date.AddDays(5);
                    if (processRequest.ComponentType == "Integral")
                    {
                        processingCharge = new Repair();
                        response1.ProcessingCharge = processingCharge.ProcessingTheService(processRequest.IsPriorityRequest);
                        if (processRequest.IsPriorityRequest)
                            response1.DateOfDelivery = DateTime.Now.Date.AddDays(2);
                    }
                    else
                    {
                        processingCharge = new Replacement();
                        response1.ProcessingCharge = processingCharge.ProcessingTheService(processRequest.IsPriorityRequest);
                    }
                    _repo.AddRequest(processRequest);
                    _repo.AddResponse(response1);
                    return Ok(response1);
                }
                else
                {
                    return BadRequest("Something went wrong");
                }
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            } 
        }

        [HttpPost]
        public ActionResult<string> CompleteProcessing(ProcessPaymentDTO paymentDTO)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:3797");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                HttpResponseMessage response = client.GetAsync("api/ProcessPayment?creditCardNumber=" + paymentDTO.CardNumber + "&creditLimit=" + paymentDTO.CardLimit + "&processingCharge=" + paymentDTO.ProcessCharge).Result;
                if (response.IsSuccessStatusCode)
                {
                    double balance = response.Content.ReadAsAsync<double>().Result;
                    if (balance >= 0)
                    {
                        //_repo.UpdataBalance(cardNumber, balance);
                        return Ok(balance.ToString());
                    }
                    else
                    {
                        return BadRequest("Insufficient Balance");
                    }
                }
                else
                    return BadRequest("Something went wrong");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("{cardNumber}")]
        public ActionResult<double> GetCardLimit(string cardNumber)
        {
            double limit = _repo.GetLimit(cardNumber);
            if (limit >= 0)
                return Ok(limit);
            else
                return BadRequest("Card number is not present");
        }
        
    }
}
