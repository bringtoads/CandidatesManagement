
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Microsoft.AspNetCore.Mvc;
using CandidatesManagement.Core.Interfaces;
using CandidatesManagement.API.Controllers;
using CandidatesManagement.Core.Models;
using CandidatesManagement.API.Models;


namespace CandidatesManagement.Tests.Controllers
{
    public class CandidatesControllerTests
    {
        private readonly Mock<ICandidateRepository> _mockRepository;
        private readonly Mock<IMemoryCache> _mockCache;
        private readonly CandidatesController _controller;

        public CandidatesControllerTests()
        {
            _mockRepository = new Mock<ICandidateRepository>();
            _mockCache = new Mock<IMemoryCache>();
            _controller = new CandidatesController(_mockRepository.Object, _mockCache.Object);
        }

        [Fact]
        public async Task UpsertCandidate_CreatesNewCandidate_WhenEmailDoesNotExist()
        {
            // Arrange
            var candidate = new Candidate { Email = "test@example.com", FirstName = "John", LastName = "Doe" };
            _mockRepository.Setup(m => m.GetByEmailAsync(It.IsAny<string>()))
                           .ReturnsAsync((Candidate)null);
            _mockRepository.Setup(m => m.AddAsync(It.IsAny<Candidate>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddOrUpdateCandidate(candidate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<Candidate>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(candidate.Email, apiResponse.Data.Email);
        }
    }
}
