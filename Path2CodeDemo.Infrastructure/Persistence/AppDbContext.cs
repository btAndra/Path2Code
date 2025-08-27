
using Microsoft.EntityFrameworkCore;
using Path2CodeDemo.Domain;

namespace Path2CodeDemo.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Candidate> Candidates { get; set; }
}