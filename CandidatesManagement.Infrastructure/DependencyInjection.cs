using CandidatesManagement.Core.Interfaces;
using CandidatesManagement.Infrastructure.Presistence;
using CandidatesManagement.Infrastructure.Presistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CandidatesManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastrructureCore(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddPresistance(configuration);

            return services;
        }

        public static IServiceCollection AddPresistance(this IServiceCollection services, IConfiguration configuration)
        {
            //if production 
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<CandidatesDbContext>(options =>
                  options.UseSqlServer(connectionString)
            );
            // else testing
            //services.AddDbContext<CandidatesDbContext>(options =>
            //options.UseInMemoryDatabase("CandidatesDb"));

            services.AddMemoryCache();
            services.AddScoped<ICandidateRepository, CandidateRepository>();
         
            return services;
        }

    }
}
