using EBay.Infrastructure;

namespace EBay.API.Infrastructure
{
    public interface IApplicationDbContextFactory : IDisposable
    {
        ApplicationDbContext Get();
    }
}