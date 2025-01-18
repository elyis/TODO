using System.Net;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Core.Entities.Request;
using Todo.Core.Entities.Response;
using Todo.Core.IService;

namespace Todo.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signup")]
        [SwaggerOperation("Регистрация пользователя")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Пользователь успешно зарегистрирован", typeof(OutputAccountCredentialsBody))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Ошибка валидации", typeof(IEnumerable<string>))]
        [SwaggerResponse((int)HttpStatusCode.Conflict, "Пользователь уже зарегистрирован", typeof(IEnumerable<string>))]
        public async Task<IActionResult> SignUp(SignUpBody body)
        {
            var response = await _authService.SignUp(body);

            if (response.IsSuccess)
                return StatusCode((int)response.StatusCode, response.Body);

            return StatusCode((int)response.StatusCode, response.Errors);
        }

        [HttpPost("signin")]
        [SwaggerOperation("Вход пользователя")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Пользователь успешно зарегистрирован", typeof(OutputAccountCredentialsBody))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Ошибка валидации или пароль неверный", typeof(IEnumerable<string>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Пользователь не найден", typeof(IEnumerable<string>))]
        public async Task<IActionResult> SignIn(SignInBody body)
        {
            var response = await _authService.SignIn(body);

            if (response.IsSuccess)
                return StatusCode((int)response.StatusCode, response.Body);

            return StatusCode((int)response.StatusCode, response.Errors);
        }

        [HttpPost("refresh")]
        [SwaggerOperation("Обновление токена доступа")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Токен доступа успешно обновлен", typeof(OutputAccountCredentialsBody))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Токен доступа не найден", typeof(IEnumerable<string>))]
        public async Task<IActionResult> Refresh(TokenBody body)
        {
            var response = await _authService.RefreshAccessToken(body.Token);
            if (response.IsSuccess)
                return StatusCode((int)response.StatusCode, response.Body);

            return StatusCode((int)response.StatusCode, response.Errors);
        }
    }
}