using System.Threading.Tasks;

namespace CineCompare.Core.Interfaces
{
    public interface IAiService
    {
        Task<string> GetMovieRecommendationAsync(string userPrompt, string userId);
    }
}