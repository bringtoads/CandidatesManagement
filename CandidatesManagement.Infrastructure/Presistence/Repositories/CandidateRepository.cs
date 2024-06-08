using CandidatesManagement.Core.Interfaces;
using CandidatesManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CandidatesManagement.Infrastructure.Presistence.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly CandidatesDbContext _context;

        public CandidateRepository(CandidatesDbContext context)
        {
            _context = context;
        }

        public async Task<Candidate> GetByEmailAsync(string email) => await _context.Candidates.FirstOrDefaultAsync(c => c.Email == email);

        public async Task AddAsync(Candidate candidate)
        {
            await _context.Candidates.AddAsync(candidate);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Candidate candidate)
        {
            var existingCandidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Email == candidate.Email);
            if (existingCandidate != null)
            {
                existingCandidate.FirstName = candidate.FirstName;
                existingCandidate.LastName = candidate.LastName;
                existingCandidate.PhoneNumber = candidate.PhoneNumber;
                existingCandidate.PreferredCallTime = candidate.PreferredCallTime;
                existingCandidate.LinkedInProfile = candidate.LinkedInProfile;
                existingCandidate.GitHubProfile = candidate.GitHubProfile;
                existingCandidate.Comment = candidate.Comment;

                _context.Candidates.Update(existingCandidate);
                await _context.SaveChangesAsync();
            }
        }
    }
}
