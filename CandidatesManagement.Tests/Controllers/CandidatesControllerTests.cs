
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Microsoft.AspNetCore.Mvc;
using CandidatesManagement.Core.Interfaces;
using CandidatesManagement.API.Controllers;
using CandidatesManagement.Core.Models;
using CandidatesManagement.API.Models;
using FluentValidation;


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
            var controller = new CandidatesController(mockRepository.Object, mockCache.Object, mockValidator.Object);
            var candidate = new Candidate
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
            var result = await controller.AddOrUpdateCandidate(candidate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<Candidate>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.NotNull(apiResponse.Data);
        }
    }
}
