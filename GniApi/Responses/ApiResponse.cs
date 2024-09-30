using System.Text.Json.Serialization;

namespace GniApi.Responses
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Item { get; set; }
        public string ErrorText { get; set; }
    }

    public class ResponseModel
    {
        [JsonPropertyName("sql_code")]
        public int SqlCode { get; set; }

        [JsonPropertyName("sql_msg")]
        public string SqlMsg { get; set; }

        [JsonPropertyName("result")]
        public ResultModel Result { get; set; }
    }

    public class ResultModel
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("requestId")]
        public int RequestId { get; set; }
    }
}
