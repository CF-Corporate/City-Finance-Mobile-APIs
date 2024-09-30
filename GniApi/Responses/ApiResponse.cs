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

    public class SqlError
    {
        [JsonPropertyName("sql_code")]
        public int SqlCode { get; set; }
        [JsonPropertyName("sql_msg")]
        public string SqlMsg { get; set; }
    }
}
