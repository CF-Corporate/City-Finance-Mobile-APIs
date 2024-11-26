using GniApi.Dtos.RequestDto.RequestDto;
using GniApi.Helper;
using GniApi.Responses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
        private string _userName;
        private string _password;


        public CustomerController(IOracleQueries oracleQueries, HttpClient httpClient, IConfiguration configuration)
        {
            this.oracleQueries = oracleQueries;
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            _configuration = configuration;
            _userName = _configuration["Oracle_Login:UserName"];
            _password = _configuration["Oracle_Login:Password"];
        }


        [HttpGet("{pin}/limit")]
        public async Task<IActionResult> Limit([FromRoute] string pin = "5ZKX6JW")
        {
            var response = await GetLimit(pin);
            if (response.StatusCode == 200)
                return StatusCode(response.StatusCode, new
                {
                    Limit = response.Item,
                    MainLimitAmount = response.MainLimitAmount
                });


            else
                return StatusCode(response.StatusCode, new
                {
                    ErrorMessage = response.ErrorText
                });
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

            var response = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_payments", new object[] { "MOBILE", _userName, _password, json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });


            return Ok(response.Result);

        }

        // needs to be discussed
        // select with PIN
        [HttpGet("{pin}/payment-history/{payment-id}")]
        public IActionResult PaymentHistoryById([FromRoute(Name = "pin")] string pin = "5ZKX6JW", [FromRoute(Name = "payment-id")] string paymentId = "336786",
                                                 string loanId = "801T09230144S01")
        {
            var json = JsonSerializer.Serialize(new { paymentId, loanId });

            var response = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_payment_info", new object[] { "MOBILE", _userName, _password, json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Ok(response.Result);
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

            var response = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_contracts", new object[] { "MOBILE", _userName, _password, json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Ok(response.Result);
        }



        [HttpGet("{pin}/loans/{loan-id}")]
        public IActionResult LoanContractsById([FromRoute(Name = "pin")] string pin = "5ZKX6JW", [FromRoute(Name = "loan-id")] string loanId = "801T09230144S01")
        {
            var json = JsonSerializer.Serialize(new { loanId });

            var response = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_contract_info", new object[] { "MOBILE", _userName, _password, json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Ok(response.Result);
        }



        [HttpGet("{pin}/loans/{loan-id}/payment-table")]
        public IActionResult PaymentTable([FromRoute(Name = "pin")] string pin = "5ZKX6JW",
                                          [FromRoute(Name = "loan-id")] string loanId = "801T09230144S01")
        {
            var json = JsonSerializer.Serialize(new { loanId });

            var response = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_payment_plan", new object[] { "MOBILE", _userName, _password, json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Ok(response.Result);

        }

        [HttpGet("{pin}/loan-requests")]
        public IActionResult LoanRequests([FromRoute(Name = "pin")] string pin = "15MRAG2",
                                            [SwaggerParameter(Description = "'Approved', 'Rejected', 'Pending', 'Offer-back', 'Offer-approved', 'Offer-rejected'")] string? status = null,
                                              DateTime? fromDate = null,
                                              DateTime? toDate = null,
                                              int page = 0,
                                              int size = 10,
                                               [SwaggerParameter(Description = "'date,ASC'")]
                                              string sort = null)
        {
            var json = JsonSerializer.Serialize(new { pin, status, fromDate = fromDate?.ToString("yyyy-MM-dd"), toDate = toDate?.ToString("yyyy-MM-dd"), page, size, sort });

            var response = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_requests", new object[] { "MOBILE", _userName, _password, json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Ok(response.Result);
        }

        [HttpGet("{pin}/loan-requests/{request-id}")]
        public async Task<IActionResult> LoanRequestById([FromRoute(Name = "pin")] string pin = "15MRAG2", [FromRoute(Name = "request-id")] int requestId = 155)
        {

            var json = JsonSerializer.Serialize(new { requestId, pin });

            var response = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_request_info", new object[] { "MOBILE", _userName, _password, json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Ok(response.Result);
        }


        [HttpPost("loan-requests/create")]
        public async Task<IActionResult> CreateLoanRequest([FromBody] RequestDto model)
        {

            int? limit = (int)(await GetLimit(model.customer.pin)).Item;
            if (limit >= model.loanRequest.amount)
            {
                model.loanRequest.limitExceeded = true;
            }
            else
            {
                model.loanRequest.limitExceeded = false;
            }
            
            var json = JsonConvert.SerializeObject(model);

            var response = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_create_request", new object[] { "MOBILE", "WS_USER", "Cf#2024@1!", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Ok(response.Result);
        }

        [HttpPost("{pin}/loan-requests/post")]
        public IActionResult PostLoanRequest([FromRoute(Name = "pin")] string pin, int requestId)
        {
            var json = JsonSerializer.Serialize(new { requestId, pin });

            var response = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_post_request",
                new object[] { "MOBILE", _userName, _password, json },
                new string[] { "p_consumer", "p_username", "p_password", "p_data" });



            return Ok(response.Result);

        }

        [HttpPost("{pin}/loan-request/offer-approve")]
        public IActionResult PostLoanRequestOfferApprove([FromRoute(Name = "pin")] string pin, int requestId)
        {
            var status = "Offer-approved";
            var json = JsonSerializer.Serialize(new { status, requestId, pin });

            var response = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_request_action",
                new object[] { "MOBILE", _userName, _password, json },
                new string[] { "p_consumer", "p_username", "p_password", "p_data" });



            return Ok(response.Result);
        }


        [HttpPost("{pin}/loan-request/offer-reject")]
        public IActionResult PostLoanRequestOfferReject([FromRoute(Name = "pin")] string pin, int requestId)
        {
            var status = "Offer-rejected";
            var json = JsonSerializer.Serialize(new { status, requestId, pin });

            var response = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_request_action",
                new object[] { "MOBILE", _userName, _password, json },
                new string[] { "p_consumer", "p_username", "p_password", "p_data" });



            return Ok(response.Result);
        }


        [HttpPost("{pin}/loans/pay")]
        public IActionResult PayLoan([FromRoute] string pin, string loanId, decimal amount, DateTime paymentDate)
        {
            var json = JsonSerializer.Serialize(new { pin, loanId, amount, paymentDate });

            var response = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_pay", new object[] { "MOBILE", _userName, _password, json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Ok(response.Result);

        }


        private async Task<ApiResponse> GetLimit(string pin)
        {
            try
            {
                var url = $"{_configuration["Url:LimitUrl"]?.ToString()}?finCode={pin}";

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

                requestMessage.Headers.Add("x-api-key", "city_finance");

                var response = await _httpClient.SendAsync(requestMessage);


                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<JsonElement>(await response.Content.ReadAsStringAsync());
                    //var mainLimitAmount = int.Parse(result.TryGetProperty("mainLimitAmount", out var mainAmountProperty)
                    //    ? mainAmountProperty.ToString() : "0");
                    var limitAmount = int.Parse(result.TryGetProperty("limitAmount", out var limitAmountProperty)
                        ? limitAmountProperty.ToString() : "0");

                    var remainingLimitAmount = int.Parse(result.TryGetProperty("remainingLimitAmount", out var remainingLimitAmountProperty)
                        ? remainingLimitAmountProperty.ToString() : "0");

                    return new()
                    {
                        StatusCode = 200,
                        Item = remainingLimitAmount > 0 ? remainingLimitAmount : 0,
                        MainLimitAmount = limitAmount > 0 ? limitAmount : 0
                    };
                }
                else
                {

                    return (int)response.StatusCode == 404 ? new ApiResponse()
                    {
                        StatusCode = 200,
                        Item = 0,
                        MainLimitAmount = 0
                    } : new()
                    {
                        StatusCode = (int)response.StatusCode,
                        ErrorText = "Error fetching data from external API"
                    };

                }
            }
            catch (TaskCanceledException)
            {
                return new()
                {
                    StatusCode = 200,
                    Item = 0,
                    MainLimitAmount = 0
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    StatusCode = 500,
                    ErrorText = $"Internal server error: {ex.Message}"
                };
            }

        }

    }
}
