
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Microsoft.AspNetCore.Mvc;
using CandidatesManagement.Core.Interfaces;
using CandidatesManagement.API.Controllers;
using CandidatesManagement.Core.Models;
using CandidatesManagement.API.Models;
using FluentValidation;
using CandidatesManagement.API.Models.Candidate;
using MapsterMapper;
using Serilog;


namespace CandidatesManagement.Tests.Controllers
{
    public class CandidatesControllerTests
    {
        [Fact]
        public async Task AddOrUpdateCandidate_ValidCandidate_ReturnsOkResult()
        {
            // Arrange
            var mockRepository = new Mock<ICandidateRepository>();
            var mockCache = new Mock<IMemoryCache>();
            var mockValidator = new Mock<IValidator<Candidate>>();
            var mockLogger = new Mock<ILogger>();

            // Mocking Mapster's IMapper (create a wrapper/mock for Mapster's mapping functionality)
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<Candidate>(It.IsAny<CandidateRequest>()))
                      .Returns((CandidateRequest candidateRequest) =>
                      {
                          // Manual mapping simulation, adjust as needed
                          return new Candidate
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
                      });

            var controller = new CandidatesController(
                mockRepository.Object,
                mockCache.Object,
                mockValidator.Object,
                mockLogger.Object,
                mockMapper.Object  // Inject mock IMapper
            );

            var candidateRequest = new CandidateRequest
            {
                Email = "john.doe@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                PreferredCallTime = "Morning",
                LinkedInProfile = "https://www.linkedin.com/in/johndoe",
                GitHubProfile = "https://github.com/johndoe",
                Comment = "This candidate has a strong background in software development."
            };

            // Act
            var result = await controller.AddOrUpdateCandidate(candidateRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<Candidate>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.NotNull(apiResponse.Data);

            // Additional assertions if needed
        }
    }
}
