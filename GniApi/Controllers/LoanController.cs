using GniApi.Dtos.RequestDto.RequestDto;
using GniApi.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GniApi.Controllers
{
    [Route("api/v1/[controller]")]
    //[ServiceFilter(typeof(HeaderCheckActionFilter))]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly IOracleQueries oracleQueries;
        private readonly IWebHostEnvironment webHostEnvironment;

        public LoanController(IOracleQueries oracleQueries, IWebHostEnvironment webHostEnvironment)
        {
            this.oracleQueries = oracleQueries;
            this.webHostEnvironment = webHostEnvironment;
        }


        [HttpGet("Loan Contracts/{pin}/loan-contracts")]
        public IActionResult GetLoanContracts(string pin, string status, DateTime fromDate, DateTime toDate, int page, int size, string sort)
        {

            var json = JsonSerializer.Serialize(new { pin, status, fromDate = fromDate.ToString("yyyy-MM-dd"), toDate = toDate.ToString("yyyy-MM-dd"), page, size, sort });



            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_contracts", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });



            return this.Content(result, "application/json");
        }

        // TESTED
        [HttpGet("Loan Contracts/{loanId}/loan-contracts")]
        public IActionResult GetLoanContractsById(string loanId)
        {

            var json = JsonSerializer.Serialize(new { loanId });



            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_contract_info", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });



            return this.Content(result, "application/json");
        }

        [HttpGet("Loan Payments/{pin}/loan-contracts")]

        public IActionResult GetLoanPayments(string pin, string loanId, int page, int size, string sort)
        {
            var json = JsonSerializer.Serialize(new { pin, loanId, page, sort, size });


            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_payments", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });


            return Content(result, "application/json");
        }

        [HttpGet("Loan Payment Info/{loanId}/loan-payment-info")]
        public IActionResult GetLoanPayments(string paymentId, string loanId)
        {
            var json = JsonSerializer.Serialize(new { paymentId, loanId });


            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_payment_info", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Content(result, "application/json");
        }

        [HttpGet("Loan Payment Plan/{loanId}/loan-payment-plan")]
        public IActionResult GetLoanPayment_plan(string loanId)
        {
            var json = JsonSerializer.Serialize(new { loanId });


            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_payment_plan", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });


            return Content(result, "application/json");
        }

        [HttpGet("Loan Requests/{pin}/loan-requests")]
        public IActionResult GetLoanRequests(string pin, string status, DateTime fromDate, DateTime toDate, int page, int size, string sort)
        {
            var json = JsonSerializer.Serialize(new { pin, status, fromDate = fromDate.ToString("yyyy-MM-dd"), toDate = toDate.ToString("yyyy-MM-dd"), page, size, sort });


            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_requests", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Content(result, "application/json");
        }

        [HttpGet("Loan Request Info/{requestId}/loan-request-info")]
        public IActionResult GetLoanRequestInfo(int requestId)
        {
            var json = JsonSerializer.Serialize(new { requestId });

            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_request_info", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Content(result, "application/json");
        }

        [HttpPost("CreateLoanRequest")]
        public IActionResult CreateLoanRequest([FromBody] RequestDto model)
        {
            var json = JsonSerializer.Serialize(model);

            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_create_request", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Content(result, "application/json");
        }

        [HttpPost("PostLoanRequest")]
        public IActionResult PostLoanRequest(int requestId)
        {
            var json = JsonSerializer.Serialize(new { requestId });

            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_post_request", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Content(result, "application/json");
        }
        [HttpPost("PayLoanRequest/{pin}")]
        public IActionResult PayLoanRequest(string pin, string loanId, decimal amount, DateTime paymentDate)
        {
            var json = JsonSerializer.Serialize(new { pin, loanId, amount, paymentDate });

            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_pay", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Content(result, "application/json");
        }




    }
}
