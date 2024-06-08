using CandidatesManagement.Core.Validators;
using FluentValidation;

namespace CandidatesManagement.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationCore(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllers();
            services.AddValidationServices();

            return services;
        }

        public static IServiceCollection AddValidationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CandidateValidator>();
            return services;
        }

        public static IApplicationBuilder UsePresentationCore(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            return app;
        }
    }
}
