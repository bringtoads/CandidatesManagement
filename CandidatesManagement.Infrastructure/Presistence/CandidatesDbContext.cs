using CandidatesManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CandidatesManagement.Infrastructure.Presistence
{
    public class CandidatesDbContext : DbContext
    {
        public CandidatesDbContext(DbContextOptions<CandidatesDbContext> options) : base (options)
        {
        }

        public DbSet<Candidate> Candidates { get; set; }
    }
}
