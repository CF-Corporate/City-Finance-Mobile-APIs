namespace GniApi.Responses;

public class BlobResponse
{
    public byte[] Data { get; set; } 
    public string ContentType { get; set; } 
    public string FileName { get; set; } 
}