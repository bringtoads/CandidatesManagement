using CandidatesManagement.Core.Models;

namespace CandidatesManagement.Core.Interfaces
{
    public interface ICandidateRepository
    {
        Task<Candidate> GetByEmailAsync(string email);
        Task AddOrUpdateAsync(Candidate candidate);
    }
}
