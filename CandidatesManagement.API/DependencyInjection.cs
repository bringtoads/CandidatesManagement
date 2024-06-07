using CandidatesManagement.Core.Validators;
using FluentValidation;

namespace CandidatesManagement.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationCore(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddValidationServices();

            return services;
        }

        public static IServiceCollection AddValidationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CandidateValidator>();
            return services;
        }
    }
}
