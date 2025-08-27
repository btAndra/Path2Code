using Path2CodeDemo.Domain;

namespace Path2CodeDemo.Application.IRepository;

public interface ICandidateRepository
{
    Task<List<Candidate>> GetAllAsync();
    Task<Candidate?> GetByIdAsync(Guid id);
    Task AddAsync(Candidate candidate);
}