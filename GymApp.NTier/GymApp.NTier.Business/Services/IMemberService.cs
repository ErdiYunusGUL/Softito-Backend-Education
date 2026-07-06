using GymApp.NTier.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymApp.NTier.Business.Services
{
    public interface IMemberService
    {
        Task<IEnumerable<Member>> GetAllMembersAsync();
        Task<Member> GetMemberByIdAsync(int id);
        Task AddMemberAsync(Member member);
        Task UpdateMemberAsync(Member member);
        Task DeleteMemberAsync(int id);
    }
}
