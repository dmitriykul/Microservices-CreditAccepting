using System;
using System.Collections.Generic;

namespace Application_acceptance_service.App
{
    public interface IRepository<T>
    {
        IEnumerable<T> Get();
        T Get(Guid id);
        Guid Create(T item);
        void Update(Guid id, T item);
        T Delete(Guid id);
    }
}