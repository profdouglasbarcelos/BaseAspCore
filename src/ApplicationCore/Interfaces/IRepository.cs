namespace ApplicationCore.Interfaces
{
    using ApplicationCore.Entities;
    using System.Collections.Generic;

    public interface IRepository<T> where T : BaseEntity
    {
        T GetById(int id);
        IEnumerable<T> ListAll();
        IEnumerable<T> List(ISpecification<T> spec);
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
