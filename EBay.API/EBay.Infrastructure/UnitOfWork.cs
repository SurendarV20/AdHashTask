using EBay.API.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EBay.API.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext _dbContext;
        protected IApplicationDbContextFactory ContextFactory { get; private set; }

        public UnitOfWork(IApplicationDbContextFactory dbContextFactory)
        {
            ContextFactory = dbContextFactory;
        }

        protected DbContext DbContext
        {
            get { return _dbContext ??= ContextFactory.Get(); }
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}