using CandidatesManagement.Core.Interfaces;
using CandidatesManagement.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace CandidatesManagement.Infrastructure.Presistence.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly CandidatesDbContext _context;
        public CandidateRepository(CandidatesDbContext context)
        {
            _context = context;
        }

        public async Task AddOrUpdateAsync(Candidate candidate)
        {
            Candidate existingCandidate = await GetByEmailAsync(candidate.Email);

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
            }
            else
            {
                await _context.Candidates.AddAsync(candidate);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Candidate> GetByEmailAsync(string email) => await _context.Candidates.SingleOrDefaultAsync(c => c.Email == email);
    }
}
