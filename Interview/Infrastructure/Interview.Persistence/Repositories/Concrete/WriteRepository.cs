namespace Interview.Persistence.Repositories.Concrete
{
    //public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    //{

    //    private readonly OnionArchitecture_DbContext _context;

    //    public WriteRepository(OnionArchitecture_DbContext context)
    //    {
    //        _context = context;
    //    }

    //    public DbSet<T> Table => _context.Set<T>();

    //    public async Task<bool> AddAsync(T entity)
    //    {

    //        EntityEntry<T> entityEntry = await Table.AddAsync(entity);
    //        return entityEntry.State == EntityState.Added;


    //    }

    //    public async Task<bool> AddRangeAsync(List<T> entities)
    //    {
    //        await Table.AddRangeAsync(entities);

    //        return true;
    //    }

    //    public bool Remove(T entity)
    //    {
    //        EntityEntry<T> entityEntry = Table.Remove(entity);

    //        return entityEntry.State == EntityState.Deleted;
    //    }

    //    public async Task<bool> RemoveByIdAsync(string id)
    //    {
    //        T model = await Table.FirstOrDefaultAsync(data => data.Id == Convert.ToInt32(id));

    //        return Remove(model);
    //    }

    //    public bool RemoveRange(List<T> entities)
    //    {
    //        Table.RemoveRange(entities);

    //        return true;
    //    }


    //    public bool Update(T entity)
    //    {
    //        EntityEntry<T> entityEntry = Table.Update(entity);

    //        return entityEntry.State == EntityState.Modified;
    //    }


    //    public async Task<int> SaveAsync()
    //    {
    //        return await _context.SaveChangesAsync();
    //    }


    //}
}
