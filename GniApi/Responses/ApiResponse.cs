namespace GniApi.Responses
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public object Item { get; set; }
        public string Error { get; set; }
    }
}
