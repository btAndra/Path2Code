namespace Path2CodeDemo.Application.RequestModels;

public class CreateCandidateRequest
{
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string ContactDetails { get; set; }
    public string? Address { get; set; }
}
