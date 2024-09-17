using GniApi.Dtos.RequestDto.RequestDto;
using GniApi.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GniApi.Controllers
{
    [Route("/api/v1/customers")]
    //[ServiceFilter(typeof(HeaderCheckActionFilter))]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IOracleQueries oracleQueries;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        //private readonly IWebHostEnvironment webHostEnvironment;

        public CustomerController(IOracleQueries oracleQueries, HttpClient httpClient, IConfiguration configuration)
        {
            this.oracleQueries = oracleQueries;
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            _configuration = configuration;
            //this.webHostEnvironment = webHostEnvironment;
        }


        // NEEDS DEVELOPMENT
        [HttpGet("{pin}/limit")]
        public async  Task<IActionResult> Limit([FromRoute] string pin = "5ZKX6JW")
        {

            try
            {
                var url = $"{_configuration["Url:LimitUrl"].ToString()}?finCode={pin}";

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

                requestMessage.Headers.Add("x-api-key", "city_finance");

                var response = await _httpClient.SendAsync(requestMessage);


                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<JsonElement>(await response.Content.ReadAsStringAsync());

                    var limitAmount = int.Parse(result.TryGetProperty("limitAmount", out var limitAmountProperty)
                 ? limitAmountProperty.ToString()
                 : "0");

                    return StatusCode(200, new
                    {
                        Limit = limitAmount
                    });
                }
                else
                {
                    return (int)response.StatusCode == 404 ? StatusCode(200, new
                    {
                        Limit = 0
                    }) : StatusCode((int)response.StatusCode, "Error fetching data from external API");

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // TESTED
        [HttpGet("{pin}/payment-history")]
        public IActionResult PaymentHistory([FromRoute(Name = "pin")] string pin = "5ZKX6JW",
                                            string loanId = "801T09230144S01",
                                            int page = 0,
                                            int size = 10,
                                            string sort = "date,ASC")
        {
            var json = JsonSerializer.Serialize(new { pin, loanId, page, sort, size });

            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_payments", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Content(result, "application/json");
        }

        // needs to be discussed
        // select with PIN
        [HttpGet("{pin}/payment-history/{payment-id}")]
        public IActionResult PaymentHistoryById([FromRoute(Name = "pin")] string pin = "5ZKX6JW",[FromRoute(Name = "payment-id")] string paymentId = "336786",
                                                 string loanId = "801T09230144S01")
        {
            var json = JsonSerializer.Serialize(new { paymentId, loanId });

            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_payment_info", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Content(result, "application/json");
        }


        [HttpGet("{pin}/loans")]
        public IActionResult Loans([FromRoute(Name = "pin")] string pin = "5ZKX6JW",
                                    string status = "Active",
                                    DateTime? fromDate = null,
                                    DateTime? toDate = null,
                                    int page = 0,
                                    int size = 10,
                                    string sort = "date,ASC")
        {
                var json = JsonSerializer.Serialize(new { pin, status, fromDate = fromDate?.ToString("yyyy-MM-dd"), toDate = toDate?.ToString("yyyy-MM-dd"), page, size, sort });

                var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_contracts", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

                return this.Content(result, "application/json");
        }



        [HttpGet("{pin}/loans/{loan-id}")]
        public IActionResult LoanContractsById([FromRoute(Name = "pin")] string pin = "5ZKX6JW",[FromRoute(Name = "loan-id")] string loanId = "801T09230144S01")
        {
            var json = JsonSerializer.Serialize(new { loanId });

            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_contract_info", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return this.Content(result, "application/json");
        }



        [HttpGet("{pin}/loans/{loan-id}/payment-table")]
        public IActionResult PaymentTable([FromRoute(Name = "pin")] string pin = "5ZKX6JW",
                                          [FromRoute(Name = "loan-id")] string loanId = "801T09230144S01")
        {
            var json = JsonSerializer.Serialize(new { loanId });

            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_payment_plan", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Content(result, "application/json");

        }

        [HttpGet("{pin}/loan-requests")]
        public IActionResult LoanRequests([FromRoute(Name = "pin")] string pin = "15MRAG2",
                                              string status = "Pending",
                                              DateTime? fromDate = null,
                                              DateTime? toDate = null,
                                              int page = 0,
                                              int size = 10,
                                              string sort = "date,ASC")
        {
            var json = JsonSerializer.Serialize(new { pin, status, fromDate = fromDate?.ToString("yyyy-MM-dd"), toDate = toDate?.ToString("yyyy-MM-dd"), page, size, sort });

            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_requests", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Content(result, "application/json");
        }

        [HttpGet("{pin}/loan-requests/{request-id}")]
        public IActionResult LoanRequestById([FromRoute(Name = "pin")] string pin = "15MRAG2" , [FromRoute(Name = "request-id")] int requestId = 155)
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
