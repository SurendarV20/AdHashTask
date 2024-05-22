using EBay.API.Domain.Interfaces;
using EBay.API.Infrastructure;
using EBay.Domain.Entities.Identity;
using EBay.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EBay.Infrastructure
{
    public static class DependancyInjection
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddDbContext<ApplicationDbContext>(opts => opts.UseSqlServer(connectionString));

            services.AddScoped(typeof(IApplicationDbContextFactory), dbContext => new DbContextFactory(connectionString, _httpContextAccessor));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            
            services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
            {
                o.User.RequireUniqueEmail = true;
            })
                  .AddEntityFrameworkStores<ApplicationDbContext>()
                  .AddDefaultTokenProviders()
                  .AddUserManager<UserManager<ApplicationUser>>();
            return services;
        }
    }
}
