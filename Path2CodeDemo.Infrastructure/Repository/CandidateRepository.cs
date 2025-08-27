using Path2CodeDemo.Domain;
using Path2CodeDemo.Application.IRepository;
using Path2CodeDemo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Path2CodeDemo.Infrastructure.Repository;

public class CandidateRepository : ICandidateRepository
{

    private readonly AppDbContext _context;
    private readonly ILogger<CandidateRepository> _logger;

    public CandidateRepository(AppDbContext context, ILogger<CandidateRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Candidate>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all candidates from the database.");
            return await _context.Candidates.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while fetching candidates: {ex.Message}");
            throw;
        }
    }

    public async Task<Candidate?> GetByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation($"Fetching candidate with ID: {id}");
            return await _context.Candidates.FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while fetching candidate with ID {id}: {ex.Message}");
            throw;
        }
    }

    public async Task AddAsync(Candidate candidate)
    {
        try
        {
            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            // Log the exception (you can use a logging framework here)
            _logger.LogError($"An error occurred while adding the candidate: {ex.Message}");
            throw; 
        }
        catch (Exception ex)
        {
            // Log any other exceptions
             _logger.LogError($"An unexpected error occurred: {ex.Message}");
            throw;
        }
    }
}
