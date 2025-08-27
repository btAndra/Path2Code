namespace Path2CodeDemo.Application.RequestModels;

public class SaveResumeRequest
{
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
    public required byte[] Content { get; set; }
}
