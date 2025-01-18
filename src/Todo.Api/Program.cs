using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Todo.App.Service;
using Todo.Core.IRepository;
using Todo.Core.IService;
using Todo.Infrastructure.Data;
using Todo.Infrastructure.Middlewares;
using Todo.Infrastructure.Repository;


var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services);

var app = builder.Build();

ConfigureMiddleware(app);

ApplyMigrations(app);

app.Run();

string GetEnvFromConfig(string key)
{
    var value = builder.Configuration[key];
    if (string.IsNullOrEmpty(value))
    {
        throw new Exception($"{key} is not set");
    }
    return value;
}

void ConfigureServices(IServiceCollection services)
{
    var jwtSecret = GetEnvFromConfig("JWT_SECRET");
    var jwtIssuer = GetEnvFromConfig("JWT_ISSUER");
    var jwtAudience = GetEnvFromConfig("JWT_AUDIENCE");
    var passwordHashKey = GetEnvFromConfig("PASSWORD_HASH_KEY");

    services.AddCors(setup =>
    {
        setup.AddDefaultPolicy(options =>
        {
            options.AllowAnyHeader();
            options.AllowAnyOrigin();
            options.AllowAnyMethod();
        });
    });

    services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience
        });

    services.AddDistributedMemoryCache();
    services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(15);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    services.AddAuthorization();

    ConfigureSwagger(services);

    services.AddDbContext<TodoDbContext>(options =>
    {
        options.UseSqlite("Data Source=Todo.db");
    });

    services.AddScoped<IRequestLogRepository, RequestLogRepository>();
    services.AddScoped<ITodoGroupRepository, TodoGroupRepository>();
    services.AddScoped<ITodoItemRepository, TodoItemRepository>();
    services.AddScoped<IAccountRepository, AccountRepository>();


    services.AddSingleton<IHashPasswordService, HashPasswordService>(sp => new HashPasswordService(passwordHashKey));
    services.AddSingleton<IJwtService, JwtService>(sp => new JwtService(
        jwtSecret,
        jwtIssuer,
        jwtAudience));


    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<ITodoGroupService, TodoGroupService>();
    services.AddScoped<ITodoItemService, TodoItemService>();
    services.AddControllers();
    services.AddEndpointsApiExplorer();
}

void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<RequestLoggingMiddleware>();
    app.UseRouting();
    app.UseCors();

    app.UseAuthentication();
    app.UseSession();
    app.UseAuthorization();

    app.MapControllers();
}

void ApplyMigrations(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TodoDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

void ConfigureSwagger(IServiceCollection services)
{
    services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Todo Api",
            Description = "Api",
        });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Bearer auth scheme",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        options.OperationFilter<SecurityRequirementsOperationFilter>();

        options.EnableAnnotations();
    });
}