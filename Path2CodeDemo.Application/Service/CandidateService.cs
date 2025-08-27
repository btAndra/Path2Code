using Path2CodeDemo.Domain;
using Path2CodeDemo.Application.IService;
using Path2CodeDemo.Application.IRepository;
using Path2CodeDemo.Application.RequestModels;
using Microsoft.Extensions.Logging;

namespace Path2CodeDemo.Application.Service;

public class CandidateService : ICandidateService
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly ILogger<CandidateService> _logger;

    public CandidateService(ICandidateRepository candidateRepository, ILogger<CandidateService> logger)
    {
        _candidateRepository = candidateRepository;
        _logger = logger;
    }

    public async Task<List<Candidate>> GetAllCandidatesAsync()
    {
        return await _candidateRepository.GetAllAsync();
    }

    public async Task<Candidate?> GetCandidateByIdAsync(Guid id)
    {
        return await _candidateRepository.GetByIdAsync(id);
    }

    public async Task<Guid> AddCandidateAsync(CreateCandidateRequest request)
    {
        var candidate = new Candidate
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            DateOfBirth = request.DateOfBirth.ToUniversalTime(),
            ContactDetails = request.ContactDetails,
            Address = request.Address
        };

        await _candidateRepository.AddAsync(candidate);

        return candidate.Id;
    }


    public Task SaveResume(SaveResumeRequest request)
    {
        var file = new Resume
        {
            FileName = request.FileName,
            ContentType = request.ContentType,
            Content = request.Content
        };

        _logger.LogInformation("Saving resume: {FileName}, ContentType: {ContentType}, Size: {Size} bytes",
            file.FileName, file.ContentType, file.Content.Length);
        // Simulate saving the file (e.g., to a database or cloud storage)
        
        return Task.CompletedTask;
       
    }
}
