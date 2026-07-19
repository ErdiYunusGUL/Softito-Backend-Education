using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineCompare.Core.Interfaces
{
    public interface IDapperRepository
    {
        // Karmaşık SQL sorgularını (JOIN'leri) içine alıp, sonucu T tipinde dönecek metot
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null);
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null);
    }
}