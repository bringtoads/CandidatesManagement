using CandidatesManagement.Core.Models;
using CandidatesManagement.Infrastructure.Presistence.Repositories;
using CandidatesManagement.Infrastructure.Presistence;
using Microsoft.EntityFrameworkCore;

namespace CandidatesManagement.Tests.Repositories
{
    public class CandidateRepositoryTests
    {
        private readonly DbContextOptions<CandidatesDbContext> _dbContextOptions;

        public CandidateRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<CandidatesDbContext>()
                .UseInMemoryDatabase(databaseName: "CandidatesTestDb")
                .Options;
        }

        [Fact]
        public async Task AddAsync_ShouldAddNewCandidate()
        {
            using var context = new CandidatesDbContext(_dbContextOptions);
            var repository = new CandidateRepository(context);
            var candidate = new Candidate { Email = "test@example.com", FirstName = "John", LastName = "Doe" };

            await repository.AddAsync(candidate);
            var result = await context.Candidates.FirstOrDefaultAsync(c => c.Email == candidate.Email);

            Assert.NotNull(result);
            Assert.Equal(candidate.Email, result.Email);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnCandidate_WhenCandidateExists()
        {
            using var context = new CandidatesDbContext(_dbContextOptions);
            var repository = new CandidateRepository(context);
            var candidate = new Candidate { Email = "test@example.com", FirstName = "John", LastName = "Doe" };

            await context.Candidates.AddAsync(candidate);
            await context.SaveChangesAsync();

            var result = await repository.GetByEmailAsync(candidate.Email);

            Assert.NotNull(result);
            Assert.Equal(candidate.Email, result.Email);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnNull_WhenCandidateDoesNotExist()
        {
            using var context = new CandidatesDbContext(_dbContextOptions);
            var repository = new CandidateRepository(context);

            var result = await repository.GetByEmailAsync("nonexistent@example.com");

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingCandidate()
        {
            using var context = new CandidatesDbContext(_dbContextOptions);
            var repository = new CandidateRepository(context);
            var candidate = new Candidate { Email = "test@example.com", FirstName = "John", LastName = "Doe" };

            await context.Candidates.AddAsync(candidate);
            await context.SaveChangesAsync();

            candidate.FirstName = "Jane";
            await repository.UpdateAsync(candidate);

            var result = await context.Candidates.FirstOrDefaultAsync(c => c.Email == candidate.Email);

            Assert.NotNull(result);
            Assert.Equal("Jane", result.FirstName);
        }
    }
}
