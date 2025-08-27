namespace Path2CodeDemo.Domain;

public class Resume
{
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
    public required byte[] Content { get; set; }
}
