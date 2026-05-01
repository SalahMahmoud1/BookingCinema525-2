using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;
namespace BookingCinema525.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected ApplicationDbContext _context; 
        DbSet<T> _dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<EntityEntry<T>> CreateAsync(T entity)
        {
            return await _dbSet.AddAsync(entity);
        }
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
        public async Task<IEnumerable<T>> GetAsync
            (
                Expression<Func<T,bool>>? filter = null,
                Expression<Func<T, object>>[]? includes = null,
                bool tracked = true
            )
        {
                var entities =_dbSet.AsQueryable();
                //Fieltere
                if (filter is not null) 
                    entities = entities.Where(filter);
                if(!tracked)
                    entities = entities.AsNoTracking();
                if(includes is not null)
                    foreach(var item in includes)
                        entities=entities.Include(item);
  
                return await entities.ToListAsync();
        }
        public async Task<T?> GetOneAsync(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>[]? includes = null,
            bool tracked = true
            )
        {
            return (await GetAsync(filter, includes, tracked)).FirstOrDefault();
        }
        public async Task<int> CommitAsync()
        {
            try
            {
                return await _context.SaveChangesAsync(); // retun no of recored changed
            }
            catch (Exception ex) 
            {
                return 0;
            }
        }
    }
}
