using System.Net;
using Todo.Core.Entities.Request;
using Todo.Core.Entities.Response;
using Todo.Core.IRepository;
using Todo.Core.IService;

namespace Todo.App.Service
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtService _jwtService;
        private readonly IHashPasswordService _hashPasswordService;

        public AuthService
        (
            IAccountRepository accountRepository,
            IJwtService jwtService,
            IHashPasswordService hashPasswordService
        )
        {
            _accountRepository = accountRepository;
            _jwtService = jwtService;
            _hashPasswordService = hashPasswordService;
        }

        public async Task<ServiceResponse<OutputAccountCredentialsBody>> RefreshAccessToken(string refreshToken)
        {
            var account = await _accountRepository.GetByTokenAsync(refreshToken);
            if (account == null)
            {
                return new ServiceResponse<OutputAccountCredentialsBody>
                {
                    Errors = new string[] { "Refresh token not found" },
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var accountCredentials = await UpdateToken(account.Id);
            return new ServiceResponse<OutputAccountCredentialsBody>
            {
                Body = accountCredentials,
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponse<OutputAccountCredentialsBody>> SignIn(SignInBody body)
        {
            var account = await _accountRepository.GetAsync(body.Email);
            if (account == null)
                return new ServiceResponse<OutputAccountCredentialsBody>
                {
                    Errors = new string[] { "Account not found" },
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound
                };

            var hashedPassword = _hashPasswordService.Compute(body.Password);
            if (account.HashPassword != hashedPassword)
            {
                return new ServiceResponse<OutputAccountCredentialsBody>
                {
                    Errors = new string[] { "Password is not correct" },
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            var outputAccountCredentials = await UpdateToken(account.Id);
            return new ServiceResponse<OutputAccountCredentialsBody>
            {
                Body = outputAccountCredentials,
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponse<OutputAccountCredentialsBody>> SignUp(SignUpBody body)
        {
            var hashedPassword = _hashPasswordService.Compute(body.Password);
            var account = await _accountRepository.AddAsync(body.Email, hashedPassword);
            if (account == null)
            {
                return new ServiceResponse<OutputAccountCredentialsBody>
                {
                    Errors = new string[] { "Account exists" },
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.Conflict
                };
            }

            var outputAccountCredentials = await UpdateToken(account.Id);
            return new ServiceResponse<OutputAccountCredentialsBody>
            {
                Body = outputAccountCredentials,
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        private async Task<OutputAccountCredentialsBody> UpdateToken(Guid accountId)
        {
            var tokenPayload = new TokenPayload
            {
                AccountId = accountId
            };

            var accountCredentials = _jwtService.GenerateOutputAccountCredentials(tokenPayload);
            accountCredentials.RefreshToken = (await _accountRepository.UpdateTokenAsync(accountId, accountCredentials.RefreshToken)).Token;
            return accountCredentials;
        }
    }
}
