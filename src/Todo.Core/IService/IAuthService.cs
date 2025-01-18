using Todo.Core.Entities.Request;
using Todo.Core.Entities.Response;

namespace Todo.Core.IService
{
    public interface IAuthService
    {
        Task<ServiceResponse<OutputAccountCredentialsBody>> SignUp(SignUpBody body);
        Task<ServiceResponse<OutputAccountCredentialsBody>> SignIn(SignInBody body);
        Task<ServiceResponse<OutputAccountCredentialsBody>> RefreshAccessToken(string refreshToken);
    }
}