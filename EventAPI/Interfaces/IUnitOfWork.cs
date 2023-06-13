namespace EventAPI.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEventRepository Events { get; }

        public int Complete();
        Task<int> CompleteAsync();
    }
}
