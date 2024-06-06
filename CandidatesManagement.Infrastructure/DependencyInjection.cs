using CandidatesManagement.Core.Interfaces;
using CandidatesManagement.Infrastructure.Presistence;
using CandidatesManagement.Infrastructure.Presistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CandidatesManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastrructureCore(this IServiceCollection services)
        {
            services.AddPresistance();

            return services;
        }

        public static IServiceCollection AddPresistance(this IServiceCollection services)
        {
            services.AddDbContext<CandidatesDbContext>(options =>
                  options.UseSqlServer("Connectionstring")
            );
            services.AddScoped<ICandidateRepository, CandidateRepository>();

            return services;
        }
    }
}
