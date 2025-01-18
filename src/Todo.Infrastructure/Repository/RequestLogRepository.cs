using Microsoft.EntityFrameworkCore;
using Todo.Core.Entities.Models;
using Todo.Core.IRepository;
using Todo.Infrastructure.Data;

namespace Todo.Infrastructure.Repository
{
    public class RequestLogRepository : IRequestLogRepository
    {
        private readonly TodoDbContext _context;

        public RequestLogRepository(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<RequestLog> CreateRequestLog(string ipAddress, string method, string path)
        {
            var requestLog = new RequestLog
            {
                IpAddress = ipAddress,
                Method = method,
                Path = path
            };
            await _context.RequestLogs.AddAsync(requestLog);
            await _context.SaveChangesAsync();
            return requestLog;
        }

        public async Task<List<RequestLog>> GetRequestLogs(int limit, int offset)
        {
            return await _context.RequestLogs.OrderByDescending(x => x.CreatedAt)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<int> GetTotalCount()
        {
            return await _context.RequestLogs.CountAsync();
        }
    }
}