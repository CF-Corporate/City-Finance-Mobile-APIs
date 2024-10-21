using GniApi.Dtos;
using GniApi.Dtos.RequestDto.RequestDto;
using GniApi.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GniApi.Controllers
{

    [Route("/api/v1/gni")]
    [ApiController]
    //[ServiceFilter(typeof(HeaderCheckActionFilter))]
    class GniController : ControllerBase
    {
        private readonly IOracleQueries oracleQueries;
        private readonly IWebHostEnvironment webHostEnvironment;

        public GniController(IOracleQueries oracleQueries, IWebHostEnvironment webHostEnvironment)
        {
            this.oracleQueries = oracleQueries;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("GetLoanContracts")]
        public IActionResult GetLoanContracts(string pin, string status, DateTime fromDate,DateTime toDate,int page, int size, string sort)
        {

            var json = JsonSerializer.Serialize(new { pin,status,fromDate=fromDate.ToString("yyyy-MM-dd"),toDate=toDate.ToString("yyyy-MM-dd"), page,size,sort});


            
            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_contracts",new object[] {"MOBILE", "HADINAJAFI", "HADI@12345" , json },new string[] { "p_consumer", "p_username", "p_password", "p_data" });



            return Ok(result);
        }
        [HttpGet("GetLoanContractsById/{loanId}")]
        public IActionResult GetLoanContractsById(string loanId)
        {

            var json = JsonSerializer.Serialize(new { loanId });



            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_contract_info", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });



            return Ok(result);
        }
        [HttpGet("GetLoanPayments")]
        public IActionResult GetLoanPayments(string pin, string loanId, int page, int size, string sort)
        {
            var json = JsonSerializer.Serialize(new { pin,loanId,page,sort,size });



            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_payments", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });






            return Ok(result);;
        }
        [HttpGet("GetLoanPaymentInfo/{loanId}")]
        public IActionResult GetLoanPayments(string paymentId, string loanId)
        {
            var json = JsonSerializer.Serialize(new {paymentId,loanId});



            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_payment_info", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });






            return Ok(result);
        }
        [HttpGet("GetLoanPaymentPlan/{loanId}")]
        public IActionResult GetLoanPayment_plan(string loanId)
        {
            var json = JsonSerializer.Serialize(new { loanId });



            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_payment_plan", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });






            return Ok(result);
        }
        [HttpGet("GetLoanRequests/{pin}")]
        public IActionResult GetLoanRequests(string pin,string status, DateTime fromDate, DateTime toDate, int page, int size, string sort)
        {
            var json = JsonSerializer.Serialize(new { pin,status,fromDate=fromDate.ToString("yyyy-MM-dd"),toDate=toDate.ToString("yyyy-MM-dd"), page,size,sort});



            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_requests", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });






            return Ok(result);;
        }
        [HttpGet("GetLoanRequestInfo/{requestId}")]
        public IActionResult GetLoanRequestInfo(int requestId)
        {
            var json = JsonSerializer.Serialize(new { requestId });



            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_request_info", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });






            return Ok(result);
        }
        [HttpPost("CreateLoanRequest")]
        public IActionResult CreateLoanRequest([FromBody] RequestDto model)
        {
            var json = JsonSerializer.Serialize(model);

            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_create_request", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });

            return Ok(result);;
        }

        [HttpPost("PostLoanRequest")]
        public IActionResult PostLoanRequest( int requestId)
        {
            var json = JsonSerializer.Serialize(new { requestId });



            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_post_request", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });






            return Ok(result);
        }
        [HttpPost("PayLoanRequest/{pin}")]
        public IActionResult PayLoanRequest( string pin,string loanId,decimal amount, DateTime paymentDate)
        {
            var json = JsonSerializer.Serialize(new { pin,loanId,amount,paymentDate });



            var result = oracleQueries.GetDataSetFromDBFunction("cfmb_loan_pay", new object[] { "MOBILE", "HADINAJAFI", "HADI@12345", json }, new string[] { "p_consumer", "p_username", "p_password", "p_data" });






            return Ok(result);;
        }
        //[HttpGet("Pdf")]
        //public async Task<IActionResult> GetPdf()
        //{
        //    string pdfUrl = "http://94.20.153.123:7777/JasperReportsIntegration/report?_repName=lombard_bas_krd_elave&_repFormat=pdf&_dataSource=avatarDS&_outFilename=&_repLocale=az_AZ&_repEncoding=UTF-8&_tsmp=52331495&tx_fid=394322";

        //    string savePath = "kaydedilecek_dosya_yolu_ve_adi.pdf";

        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            HttpResponseMessage response = await client.GetAsync(pdfUrl);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                using (Stream pdfStream = await response.Content.ReadAsStreamAsync())
        //                using (FileStream fileStream = System.IO.File.Create(Path.Combine(webHostEnvironment.WebRootPath,"awda.pdf")))
        //                {
        //                    await pdfStream.CopyToAsync(fileStream);
        //                }

        //                Console.WriteLine("PDF dosyası başarıyla kaydedildi.");
        //            }
        //            else
        //            {
        //                Console.WriteLine("PDF dosyası alınırken bir hata oluştu. HTTP durumu: " + response.StatusCode);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Bir hata oluştu: " + ex.Message);
        //        }
        //        return Ok();
        //    }
        //}

    }
}
