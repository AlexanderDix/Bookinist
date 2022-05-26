using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bookinist.DAL.Context;
using Bookinist.DAL.Entities.Base;
using Bookinist.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookinist.DAL;

internal class DbRepository<T> : IRepository<T> where T : Entity, new()
{
    private readonly BookinistDb _db;
    private readonly DbSet<T> _set;

    public bool AutoSaveChanges { get; set; } = true;

    public DbRepository(BookinistDb db)
    {
        _db = db;
        _set = _db.Set<T>();
    }

    public virtual IQueryable<T> Items => _set;

    public T Get(int id) => Items.SingleOrDefault(item => item.Id == id);

    public async Task<T> GetAsync(int id, CancellationToken cancellationToken = default) => await Items
        .SingleOrDefaultAsync(item => item.Id == id, cancellationToken)
        .ConfigureAwait(false);

    public T Add(T item)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));
        _db.Entry(item).State = EntityState.Added;
        if (AutoSaveChanges)
            _db.SaveChanges();
        return item;
    }

    public async Task<T> AddAsync(T item, CancellationToken cancellationToken = default)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));
        _db.Entry(item).State = EntityState.Added;
        if (AutoSaveChanges)
            await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return item;
    }

    public void Update(T item)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));
        _db.Entry(item).State = EntityState.Modified;
        if (AutoSaveChanges)
            _db.SaveChanges();
    }

    public async Task UpdateAsync(T item, CancellationToken cancellationToken = default)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));
        _db.Entry(item).State = EntityState.Modified;
        if (AutoSaveChanges)
            await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public void Remove(int id)
    {
        T item = _set.Local.FirstOrDefault(i => i.Id == id) ?? new T {Id = id};

        _db.Remove(item);
        if (AutoSaveChanges)
            _db.SaveChanges();
    }

    public async Task RemoveAsync(int id, CancellationToken cancellationToken = default)
    {
        _db.Remove(new T {Id = id});
        if (AutoSaveChanges)
            await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}