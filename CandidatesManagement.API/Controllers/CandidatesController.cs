using CandidatesManagement.API.Models;
using CandidatesManagement.API.Models.Candidate;
using CandidatesManagement.Core.Interfaces;
using CandidatesManagement.Core.Models;
using FluentValidation;
using FluentValidation.Results;
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
        private readonly IValidator<Candidate> _validator;
        private readonly Serilog.ILogger _logger;

        public CandidatesController(
            ICandidateRepository candidateRepository,
            IMemoryCache cache,
            IValidator<Candidate> validator,
            Serilog.ILogger logger)
        {
            _candidateRepository = candidateRepository;
            _cache = cache;
            _validator = validator;
            _logger = logger;
        }

        [HttpPost("AddOrUpdateCandidate")]
        public async Task<IActionResult> AddOrUpdateCandidate([FromBody] CandidateRequest candidateRequest)
        {
            var candidate = new Candidate
            {
                FirstName = candidateRequest.FirstName,
                LastName = candidateRequest.LastName,
                PhoneNumber = candidateRequest.PhoneNumber,
                Email = candidateRequest.Email,
                PreferredCallTime = candidateRequest.PreferredCallTime,
                LinkedInProfile = candidateRequest.LinkedInProfile,
                GitHubProfile = candidateRequest.GitHubProfile,
                Comment = candidateRequest.Comment,
            };
            ValidationResult result = await _validator.ValidateAsync(candidate);
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToArray();
                return BadRequest(new ApiResponse<string[]>(false, "Validation errors occurred", errors));
            }

            try
            {
                var cacheKey = $"Candidate_{candidate.Email}";
                Candidate existingCandidate = null;

                if (!_cache.TryGetValue(cacheKey, out existingCandidate))
                {
                    existingCandidate = await _candidateRepository.GetByEmailAsync(candidate.Email);
                }

                if (existingCandidate != null)
                {
                    candidate.Id = existingCandidate.Id;
                    await _candidateRepository.UpdateAsync(candidate);
                    _cache.Set(cacheKey, candidate, TimeSpan.FromMinutes(30));

                    return Ok(new ApiResponse<Candidate>(true, "Candidate updated successfully", candidate));
                }
                else
                {
                    await _candidateRepository.AddAsync(candidate);

                    _cache.Set(cacheKey, candidate, TimeSpan.FromMinutes(30));

                    return Ok(new ApiResponse<Candidate>(true, "Candidate added successfully", candidate));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in {Method}", nameof(AddOrUpdateCandidate));
                return StatusCode(500, new ApiResponse<string>(false, $"An error occurred: {ex.Message}"));
            }
        }
    }
}
