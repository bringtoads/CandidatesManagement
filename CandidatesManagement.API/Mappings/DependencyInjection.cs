using Mapster;
using MapsterMapper;
using System.Reflection;

namespace CandidatesManagement.API.Mappings
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMappingsCore(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }
    }
}
