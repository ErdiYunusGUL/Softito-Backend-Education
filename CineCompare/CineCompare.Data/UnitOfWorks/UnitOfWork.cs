using CineCompare.Core.Interfaces;
using CineCompare.Data.Contexts;
using System.Threading.Tasks;

namespace CineCompare.Data.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CineCompareDbContext _context;

        public UnitOfWork(CineCompareDbContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}