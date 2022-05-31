using System.Collections.Generic;

namespace HabitApp.Repositories
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        T GetById(int id);
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);
    }
}
