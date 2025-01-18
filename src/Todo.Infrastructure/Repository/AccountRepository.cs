using Microsoft.EntityFrameworkCore;
using Todo.Core.Entities.Models;
using Todo.Core.IRepository;
using Todo.Infrastructure.Data;

namespace Todo.Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly TodoDbContext _context;

        public AccountRepository(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<Account?> AddAsync(string email, string passwordHash)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(e => e.Email == email);
            if (account != null)
                return null;

            account = new Account
            {
                Email = email,
                HashPassword = passwordHash,
            };

            account = (await _context.Accounts.AddAsync(account))?.Entity;
            await _context.SaveChangesAsync();

            return account;
        }

        public async Task<Account?> GetByTokenAsync(string refreshToken)
        {
            return await _context.Accounts.FirstOrDefaultAsync(e => e.Token == refreshToken);
        }

        public async Task<Account?> UpdateTokenAsync(Guid id, string token, DateTime? tokenValidBefore = null)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(e => e.Id == id);
            if (account == null)
                return null;

            if (tokenValidBefore == null)
                tokenValidBefore = DateTime.UtcNow.Add(TimeSpan.FromMinutes(10));

            if (account.TokenValidBefore == null || account.TokenValidBefore <= DateTime.UtcNow)
            {
                account.TokenValidBefore = tokenValidBefore;
                account.Token = token;
                await _context.SaveChangesAsync();
            }

            return account;
        }

        public async Task<Account?> GetAsync(Guid id)
        {
            return await _context.Accounts.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Account?> GetAsync(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(e => e.Email == email);
        }
    }
}