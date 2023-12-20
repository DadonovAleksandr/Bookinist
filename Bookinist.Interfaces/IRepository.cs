namespace Bookinist.Interfaces;

public interface IRepository<T> where T : class, IEntity, new()
{
    IQueryable<T> Items { get; }

    T Get(int id);
    T Add(T item);
    void Updater(T item);
    void Remove(int id);

    Task<T> GetAsync(int id, CancellationToken cancellation = default);
    Task<T> AddAsync(T item, CancellationToken cancellation = default);
    Task UpdaterAsync(T item, CancellationToken cancellation = default);
    Task RemoveAsync(int id, CancellationToken cancellation = default);
}
