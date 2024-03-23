using GniApi.Dtos.RequestDto.RequestDto;
using GniApi.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GniApi.Controllers
{
    [Route("/api/v1/customers")]
    [ServiceFilter(typeof(HeaderCheckActionFilter))]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IOracleQueries oracleQueries;

        //private readonly IWebHostEnvironment webHostEnvironment;

        public CustomerController(IOracleQueries oracleQueries)
        {
            this.oracleQueries = oracleQueries;
            //this.webHostEnvironment = webHostEnvironment;
        }


        // NEEDS DEVELOPMENT
        [HttpGet("{pin}/limit")]
        public IActionResult Limit([FromRoute] string pin)
        {
            return Ok("""
                {
                    "limit": 5000
                }
                """);
            // Use the customer_fincode parameter here
            // Example: return Ok($"Fincode: {customer_fincode}");
        }

        // TESTED
        [HttpGet("{pin}/payment-history")]
        public IActionResult PaymentHistory([FromRoute(Name = "payment-id")] string pin, 
                                            string loanId, 
                                            int page, 
                                            int size, 
                                            string sort)
        {
            var json = JsonSerializer.Serialize(new { pin, loanId, page, sort, size });

            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_payments", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Content(result, "application/json");
        }

        // needs to be discussed
        // select with PIN
        [HttpGet("{pin}/payment-history/{payment-id}")]
        public IActionResult PaymentHistoryById([FromRoute(Name = "payment-id")] string paymentId,
                                                 string loanId)
        {
            var json = JsonSerializer.Serialize(new { paymentId, loanId });

            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_payment_info", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Content(result, "application/json");
        }


        [HttpGet("{pin}/loans")]
        public IActionResult Loans([FromRoute(Name = "pin")] string pin, 
                                    string status, 
                                    DateTime fromDate, 
                                    DateTime toDate, 
                                    int page, 
                                    int size, 
                                    string sort)
        {
            var json = JsonSerializer.Serialize(new { pin, status, fromDate = fromDate.ToString("yyyy-MM-dd"), toDate = toDate.ToString("yyyy-MM-dd"), page, size, sort });

            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_contracts", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return this.Content(result, "application/json");
        }



        [HttpGet("{pin}/loans/{loan-id}")]
        public IActionResult LoanContractsById([FromRoute(Name = "loan-id")] string loanId)
        {
            var json = JsonSerializer.Serialize(new { loanId });

            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_contract_info", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return this.Content(result, "application/json");
        }



        [HttpGet("{pin}/loans/{loan-id}/payment-table")]
        public IActionResult PaymentTable([FromRoute(Name = "pin")] string pin,
                                          [FromRoute(Name = "loan-id")] string loanId)
        {
            var json = JsonSerializer.Serialize(new { loanId });

            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_payment_plan", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Content(result, "application/json");

        }

        [HttpGet("{pin}/loan-requests")]
        public IActionResult LoanRequests([FromRoute(Name = "pin")] string pin, 
                                              string status, 
                                              DateTime fromDate,
                                              DateTime toDate, 
                                              int page, 
                                              int size, 
                                              string sort)
        {
            var json = JsonSerializer.Serialize(new { pin, status, fromDate = fromDate.ToString("yyyy-MM-dd"), toDate = toDate.ToString("yyyy-MM-dd"), page, size, sort });

            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_requests", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Content(result, "application/json");
        }

        [HttpGet("{pin}/loan-requests/{request-id}")]
        public IActionResult LoanRequestById([FromRoute(Name = "request-id")]int requestId)
        {
            var json = JsonSerializer.Serialize(new { requestId });

            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_request_info", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Content(result, "application/json");
        }


        [HttpPost("{pin}/loan-requests/create")]
        public IActionResult CreateLoanRequest([FromBody] RequestDto model)
        {
            var json = JsonSerializer.Serialize(model);

            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_create_request", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Content(result, "application/json");
        }

        // has error
        [HttpPost("{pin}/loan-requests/post")]
        public IActionResult PostLoanRequest(int requestId)
        {
            var json = JsonSerializer.Serialize(new { requestId });

            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_post_request",
                new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, 
                new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Content(result, "application/json");
        }





    }

}
