using System;
using System.Threading.Tasks;

namespace CineCompare.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync(); // Değişiklikleri kaydet
        void Commit();
    }
}