using System.Text;
using System.Text.Json.Serialization;
using IdentityService.API.Middlewares;
using IdentityService.BLL.Services.Implements;
using IdentityService.BLL.Services.Interfaces;
using IdentityService.BLL.Utils;
using IdentityService.DAL.Models;
using IdentityService.DAL.Repositories.Implements;
using IdentityService.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// --------------------- Configure Services ---------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Register Repositories
builder.Services.AddScoped<IUnitOfWork<IdentityDbContext>, UnitOfWork<IdentityDbContext>>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Register Utilities
builder.Services.AddSingleton<OtpUtil>();
builder.Services.AddSingleton<IJwtUtil, JwtUtil>();

// Register Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRefreshTokensService, RefreshTokensService>();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(accessToken) && !accessToken.StartsWith("Bearer "))
                {
                    context.Request.Headers["Authorization"] = "Bearer " + accessToken;
                }
                return Task.CompletedTask;
            },
            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    StatusCode = 401,
                    Message = "Unauthorized access",
                    Path = context.Request.Path,
                    Method = context.Request.Method,
                    Timestamp = DateTime.UtcNow
                });
            }
        };
    });

builder.Services.AddAuthorization();

// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "IdentityService API",
        Version = "v1",
        Description = "Authentication and Authorization Microservice"
    });

    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// --------------------- Build App ---------------------
var app = builder.Build();

// --------------------- Middleware ---------------------
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ResetPasswordOnlyMiddleware>();

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.SetIsOriginAllowed(origin =>
       origin.StartsWith("http://localhost:") ||
       origin.StartsWith("https://localhost:") ||
       origin.EndsWith(".vercel.app"))
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
