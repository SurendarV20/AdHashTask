namespace EBay.API.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        void Commit();

        Task CommitAsync();
    }
}