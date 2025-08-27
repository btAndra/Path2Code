namespace Path2CodeDemo.Domain;

public class Candidate
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string ContactDetails { get; set; }
    public string? Address { get; set; }

    public string? Version { get; set; }
} 
