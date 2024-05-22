using EBay.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EBay.API.Infrastructure
{
    public class DbContextFactory : IApplicationDbContextFactory
    {
        private readonly string _connectionString;
        private readonly bool _useManagedIdentity;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private ApplicationDbContext _dbContext;

        //public DbContextFactory(string connectionString, AzureAuthenticationInterceptor azureAuthenticationInterceptor = null)
        public DbContextFactory(string connectionString, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = connectionString;

            _useManagedIdentity = true;
            if (connectionString.ToLower().Contains("local"))
            {
                _useManagedIdentity = false;
            }
            _httpContextAccessor = httpContextAccessor;
        }

        public ApplicationDbContext Get()
        {
            DbContextOptions<ApplicationDbContext> options;
            if (_dbContext != null)
            {
                return _dbContext;
            }
            else
            {

                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder
                    .UseSqlServer(_connectionString);
                options = optionsBuilder.Options;
            }
            _dbContext = new ApplicationDbContext(options, _httpContextAccessor);
            return _dbContext;
        }

        public void Dispose()
        {
            _dbContext = null;
        }
    }
}