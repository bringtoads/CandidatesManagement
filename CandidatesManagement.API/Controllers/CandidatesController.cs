using CandidatesManagement.API.Models;
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

        public CandidatesController(
            ICandidateRepository candidateRepository,
            IMemoryCache cache,
            IValidator<Candidate> validator)
        {
            _candidateRepository = candidateRepository;
            _cache = cache;
            _validator = validator;
        }

        [HttpPost("AddOrUpdateCandidate")]
        public async Task<IActionResult> AddOrUpdateCandidate([FromBody] Candidate candidate)
        {
            ValidationResult result = await _validator.ValidateAsync(candidate);
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToArray();
                return BadRequest(new ApiResponse<string[]>(false, "Validation errors occurred", errors));
            }

            try
            {
                var existingCandidate = await _candidateRepository.GetByEmailAsync(candidate.Email);
                if (existingCandidate != null)
                {
                    candidate.Id = existingCandidate.Id; // Ensure the ID remains unchanged
                    await _candidateRepository.UpdateAsync(candidate);
                    return Ok(new ApiResponse<Candidate>(true, "Candidate updated successfully", candidate));
                }
                else
                {
                    await _candidateRepository.AddAsync(candidate);
                    return Ok(new ApiResponse<Candidate>(true, "Candidate added successfully", candidate));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(false, $"An error occurred: {ex.Message}"));
            }
        }
    }
}
