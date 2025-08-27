using Path2CodeDemo.Domain;
using Path2CodeDemo.Application.RequestModels;

namespace Path2CodeDemo.Application.IService;

public interface ICandidateService
{
    Task<List<Candidate>> GetAllCandidatesAsync();
    Task<Candidate?> GetCandidateByIdAsync(Guid id);
    Task<Guid> AddCandidateAsync(CreateCandidateRequest request);
    Task SaveResume(SaveResumeRequest request);
}