using Bookinist.DAL.Context;
using Bookinist.DAL.Entityes.Base;
using Bookinist.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookinist.DAL;

internal class DBRepository<T> : IRepository<T> where T : Entity, new()
{
    private readonly BookinistDB _db;
    private readonly DbSet<T> _set;

    public bool AutoSaveChange {  get; set; } = true;

    public DBRepository(BookinistDB db)
    {
        _db = db;
        _set = db.Set<T>();
    }

    public virtual IQueryable<T> Items => _set;

    public T Add(T item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));
        _db.Entry(item).State = EntityState.Added;
        if(AutoSaveChange)
            _db.SaveChanges();
        return item;
    }

    public async Task<T> AddAsync(T item, CancellationToken cancellation = default)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));
        _db.Entry(item).State = EntityState.Added;
        if (AutoSaveChange)
            _db.SaveChangesAsync(cancellation).ConfigureAwait(false);
        return item;
    }

    public T Get(int id) => Items.SingleOrDefault(x => x.Id == id);

    public async Task<T> GetAsync(int id, CancellationToken cancellation = default) => await Items
        .SingleOrDefaultAsync(x => x.Id == id, cancellation)
        .ConfigureAwait(false);

    public void Remove(int id)
    {
        _db.Remove(new T { Id = id });
        if(AutoSaveChange) 
            _db.SaveChanges();
    }

    public async Task RemoveAsync(int id, CancellationToken cancellation = default)
    {
        _db.Remove(new T { Id = id });
        if (AutoSaveChange)
            _db.SaveChanges();
    }

    public void Updater(T item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));
        _db.Entry(item).State = EntityState.Modified;
        if (AutoSaveChange)
            _db.SaveChanges();
    }

    public async Task UpdaterAsync(T item, CancellationToken cancellation = default)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));
        _db.Entry(item).State = EntityState.Modified;
        if (AutoSaveChange)
            _db.SaveChangesAsync(cancellation).ConfigureAwait(false);
    }
}