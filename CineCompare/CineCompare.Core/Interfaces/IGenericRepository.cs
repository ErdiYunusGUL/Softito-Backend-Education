using CineCompare.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineCompare.Core.Interfaces
{
    // "T" mutlaka BaseEntity'den miras alan bir sınıf olmalı kuralı (Güvenlik için)
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}