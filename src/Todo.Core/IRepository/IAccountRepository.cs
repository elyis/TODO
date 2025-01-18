using Todo.Core.Entities.Models;


namespace Todo.Core.IRepository
{
    public interface IAccountRepository
    {
        Task<Account?> AddAsync(string email, string passwordHash);
        Task<Account?> GetByTokenAsync(string refreshToken);
        Task<Account?> UpdateTokenAsync(Guid id, string token, DateTime? tokenValidBefore = null);
        Task<Account?> GetAsync(Guid id);
        Task<Account?> GetAsync(string email);
    }
}