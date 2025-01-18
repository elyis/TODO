using Todo.Core.Entities.Response;

namespace Todo.Core.IService
{
    public interface IJwtService
    {
        OutputAccountCredentialsBody GenerateOutputAccountCredentials(TokenPayload tokenPayload);
        TokenPayload GetTokenPayload(string token);
    }
}