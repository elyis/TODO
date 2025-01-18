using Todo.Core.Entities.Models;

namespace Todo.Core.IRepository
{
    public interface IRequestLogRepository
    {
        Task<RequestLog> CreateRequestLog(string ipAddress, string method, string path);
        Task<List<RequestLog>> GetRequestLogs(int limit, int offset);
        Task<int> GetTotalCount();
    }
}