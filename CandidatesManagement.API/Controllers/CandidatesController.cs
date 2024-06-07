using CandidatesManagement.API.Models;
using CandidatesManagement.Core.Interfaces;
using CandidatesManagement.Core.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CandidatesManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IMemoryCache _cache;

        public CandidatesController(
            ICandidateRepository candidateRepository,
            IMemoryCache cache)
        {
            _candidateRepository = candidateRepository;
            _cache = cache;
        }

        [HttpPost("AddOrUpdateCandidate")]
        public async Task<IActionResult> AddOrUpdateCandidate([FromBody] Candidate candidate)
        {
            {
                try
                {
                    var cachedCandidate = _cache.Get<Candidate>(candidate.Email);
                    if (cachedCandidate != null)
                    {
                        candidate.Adapt(cachedCandidate);
                        await _candidateRepository.UpdateAsync(cachedCandidate);
                        _cache.Set(candidate.Email, cachedCandidate);
                        return Ok(new ApiResponse<Candidate>(true, "Candidate updated successfully", cachedCandidate));
                    }
                    
                    var existingCandidate = await _candidateRepository.GetByEmailAsync(candidate.Email);
                    if (existingCandidate != null)
                    {
                        candidate.Adapt(existingCandidate);
                        await _candidateRepository.UpdateAsync(existingCandidate);
                        _cache.Set(candidate.Email, existingCandidate);
                        return Ok(new ApiResponse<Candidate>(true, "Candidate updated successfully", existingCandidate));
                    }

                    await _candidateRepository.AddAsync(candidate);
                    _cache.Set(candidate.Email, candidate);
                    return Ok(new ApiResponse<Candidate>(true, "Candidate added successfully", candidate));
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new ApiResponse<string>(false, $"An error occurred: {ex.Message}"));
                }
            }
        }
    }
}
