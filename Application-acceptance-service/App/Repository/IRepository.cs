using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application_acceptance_service.App
{
    public interface IRepository<T>
    {
        IEnumerable<T> Get();
        Task<T> Get(Guid id);
        Task<Guid> Create(T item);
        void Update(Guid id, T item);
        Task<T>Delete(Guid id);
    }
}