using EBay.Domain;
using EBay.Service;
using Microsoft.Extensions.DependencyInjection;

namespace EBay.Infrastructure
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IVehicleService, VehicleService>();
            return services;
        }
    }
}
