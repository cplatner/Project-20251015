using StargateAPI.Business.Repositories;

namespace StargateAPI;

public static class AddRepositoriesExtension
{
    /// <summary>
    /// Add repositories to DI
    /// </summary>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IPeopleRepository, PeopleRepository>();
        services.AddTransient<IAstronautDutyRepository, AstronautDutyRepository>();
        return services;
    }
}