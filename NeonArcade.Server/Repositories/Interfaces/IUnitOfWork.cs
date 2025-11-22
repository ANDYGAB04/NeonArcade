namespace NeonArcade.Server.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGameRepository Games { get; }
        IOrderRepository Orders { get; }
        ICartRepository Carts { get; }
        Task<int> SaveChangesAsync();

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
