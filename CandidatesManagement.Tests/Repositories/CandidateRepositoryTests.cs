using CandidatesManagement.Core.Models;
using CandidatesManagement.Infrastructure.Presistence.Repositories;
using CandidatesManagement.Infrastructure.Presistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CandidatesManagement.Tests.Repositories
{
    public class CandidateRepositoryTests
    {
        [Fact]
        public async Task AddAsync_Adds_New_Candidate()
        {
            // Arrange
            var candidate = new Candidate
            {
                Email = "test3@example.com",
                FirstName = "Alice",
                LastName = "Johnson"
            };

            var mockDbSet = new Mock<DbSet<Candidate>>();
            mockDbSet.Setup(x => x.AddAsync(candidate, default)).ReturnsAsync(Mock.Of<EntityEntry<Candidate>>(_ => _.Entity == candidate));

            var mockDbContext = new Mock<CandidatesDbContext>();
            mockDbContext.Setup(x => x.Candidates).Returns(mockDbSet.Object);

            var repository = new CandidateRepository(mockDbContext.Object);

            // Act
            await repository.AddAsync(candidate);

            // Assert
            mockDbSet.Verify(x => x.AddAsync(candidate, default), Times.Once);
            mockDbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Does_Not_Update_Nonexistent_Candidate()
        {
            // Arrange
            var candidate = new Candidate
            {
                Email = "nonexistent@example.com",
                FirstName = "Non",
                LastName = "Existent"
            };

            var mockDbSet = new Mock<DbSet<Candidate>>();
            mockDbSet.Setup(x => x.FindAsync(It.IsAny<object[]>())).ReturnsAsync((Candidate)null);

            var mockDbContext = new Mock<CandidatesDbContext>();
            mockDbContext.Setup(x => x.Candidates).Returns(mockDbSet.Object);

            var repository = new CandidateRepository(mockDbContext.Object);

            // Act
            await repository.UpdateAsync(candidate);

            // Assert
            mockDbSet.Verify(x => x.Update(It.IsAny<Candidate>()), Times.Never);
            mockDbContext.Verify(x => x.SaveChangesAsync(default), Times.Never);
        }
    }
}
