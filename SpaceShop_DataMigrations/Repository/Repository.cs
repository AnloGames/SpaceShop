using Microsoft.EntityFrameworkCore;
using SpaceShop_DataMigrations.Repository.IRepository;
using System.Linq.Expressions;

namespace SpaceShop_DataMigrations.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext db;
        private DbSet<T> dbSet;
      
        public Repository(ApplicationDbContext db)
        {
            this.db = db;
            dbSet = this.db.Set<T>();
        }

        public void Add(T item)
        {
            dbSet.Add(item);
        }

        public T Find(int id)
        {
            return dbSet.Find(id);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter = null, 
            string includeProperties = null, 
            bool isTracking = true)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(','))
                {
                    query.Include(item);
                }
            }
            if (!isTracking)
            {
                query.AsNoTracking();
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, 
            IOrderedQueryable<T>> orderBy = null, 
            string includeProperties = null,
            bool isTracking = true)
        {
            IQueryable<T> query = dbSet;

            if (filter != null) 
            {
                query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(','))
                {
                    query.Include(item);
                }
            }
            if (orderBy != null) 
            {
                query = orderBy(query);
            }
            if (!isTracking)
            {
                query.AsNoTracking();
            }

            return query.ToList();
        }

        public void Remove(T item)
        {
            dbSet.Remove(item);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
