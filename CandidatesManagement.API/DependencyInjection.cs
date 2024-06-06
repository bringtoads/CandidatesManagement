namespace CandidatesManagement.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationCore(this IServiceCollection services)
        {
            services.AddControllers();
            return services;
        }
    }
}
