using CandidatesManagement.Core.Interfaces;
using CandidatesManagement.Core.Models;
using Microsoft.AspNetCore.Http;
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

        public CandidatesController(ICandidateRepository candidateRepository, IMemoryCache cache)
        {
            _candidateRepository = candidateRepository;
            _cache = cache;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateCandidate([FromBody] Candidate candidate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cacheKey = $"Candidate_{candidate.Email}";
            _cache.Remove(cacheKey); // Invalidate cache

            await _candidateRepository.AddOrUpdateAsync(candidate);

            // Cache the updated candidate
            _cache.Set(cacheKey, candidate, TimeSpan.FromMinutes(5));

            return Ok(candidate);
        }
    }
}
