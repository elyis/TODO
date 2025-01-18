using System.Net;
using Todo.Core.Entities.Request;
using Todo.Core.Entities.Response;
using Todo.Core.IRepository;
using Todo.Core.IService;

namespace Todo.App.Service
{
    public class TodoGroupService : ITodoGroupService
    {
        private readonly IJwtService _jwtService;
        private readonly ITodoGroupRepository _todoGroupRepository;
        private readonly IAccountRepository _accountRepository;

        public TodoGroupService(
            ITodoGroupRepository todoGroupRepository,
            IAccountRepository accountRepository,
            IJwtService jwtService)
        {
            _todoGroupRepository = todoGroupRepository;
            _accountRepository = accountRepository;
            _jwtService = jwtService;
        }

        public async Task<ServiceResponse<TodoGroupBody>> CreateTodoGroup(CreateTodoGroupBody body, string token)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var account = await _accountRepository.GetAsync(tokenPayload.AccountId);
            if (account == null)
                return new ServiceResponse<TodoGroupBody>
                {
                    Body = null,
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.Unauthorized
                };

            var todoGroup = await _todoGroupRepository.AddAsync(body, account);
            return new ServiceResponse<TodoGroupBody>
            {
                Body = todoGroup?.ToTodoGroupBody(),
                IsSuccess = todoGroup != null,
                StatusCode = todoGroup == null ? HttpStatusCode.InternalServerError : HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponse<TodoGroupBody>> GetTodoGroup(Guid id)
        {
            var todoGroup = await _todoGroupRepository.GetAsync(id);
            return new ServiceResponse<TodoGroupBody>
            {
                Body = todoGroup?.ToTodoGroupBody(),
                IsSuccess = todoGroup != null,
                StatusCode = todoGroup == null ? HttpStatusCode.NotFound : HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponse<TodoGroupBody>> UpdateTodoGroupName(UpdateTodoGroupBody body)
        {
            var todoGroup = await _todoGroupRepository.UpdateNameAsync(body.Id, body.Name);
            return new ServiceResponse<TodoGroupBody>
            {
                Body = todoGroup?.ToTodoGroupBody(),
                IsSuccess = todoGroup != null,
                StatusCode = todoGroup == null ? HttpStatusCode.NotFound : HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponse<TodoGroupBody>> DeleteTodoGroup(Guid id)
        {
            await _todoGroupRepository.DeleteAsync(id);
            return new ServiceResponse<TodoGroupBody>
            {
                Body = null,
                IsSuccess = true,
                StatusCode = HttpStatusCode.NoContent
            };
        }

        public async Task<ServiceResponse<PaginationResponse<TodoGroupBody>>> GetTodoGroups(int limit, int offset, string token)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var todoGroups = await _todoGroupRepository.GetAllByAccountIdAsync(limit, offset, tokenPayload.AccountId);
            var totalCount = await _todoGroupRepository.GetTotalCountByAccountIdAsync(tokenPayload.AccountId);
            return new ServiceResponse<PaginationResponse<TodoGroupBody>>
            {
                Body = new PaginationResponse<TodoGroupBody>
                {
                    Items = todoGroups.Select(tg => tg.ToTodoGroupBody()),
                    Limit = limit,
                    Offset = offset,
                    TotalCount = totalCount
                },
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}