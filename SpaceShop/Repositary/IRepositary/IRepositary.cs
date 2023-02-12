using System.Linq.Expressions;

namespace SpaceShop.Repository.IRepositary
{
    public interface IRepositary<T> where T: class
    {
        T Find(int id);
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null
            );
        void Add(T item);

        void Remove(T item);

        void Update(T item);    //Изменить потом

        void Save();
    }
}
