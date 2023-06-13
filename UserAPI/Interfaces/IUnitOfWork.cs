namespace UserAPI.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        public int Complete();
        Task<int> CompleteAsync();
    }
}
