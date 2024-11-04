using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

using Domain.Interfaces;
using Infrastructure.Data;
using Vocab.Infrastructure.Data;
using Application.Mappings;
using Application.Interfaces;
using Application.Services;
using Application.UseCases.JWT;
using Application.UseCases.Auth;
using Application.UseCases.Tracks;

namespace Endpoint.Extensions;

public static class AppServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConfiguration = configuration.GetConnectionString("RedisConnection");

        if (string.IsNullOrEmpty(redisConfiguration))
        {
            throw new ArgumentNullException(nameof(redisConfiguration), "Redis connection string cannot be null or empty.");
        }

        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfiguration));

        services.AddScoped<IRedisCache, RedisCache>();

        services.AddAutoMapper(typeof(MappingProfile));

        services.AddScoped<GenerateTokenPairUseCase>();
        services.AddScoped<RenewAccessTokenUseCase>();
        services.AddScoped<IJWTService, JwtService>();

        services.AddScoped<LoginUseCase>();
        services.AddScoped<RegisterUseCase>();
        services.AddScoped<ConfirmRegistrationEmailUseCase>();
        services.AddScoped<ForgotPasswordUseCase>();
        services.AddScoped<ConfirmPasswordResettingEmailUseCase>();
        services.AddScoped<LogoutUseCase>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<CreateDeleteTrackSubjectUseCase>();
        services.AddScoped<GetDetailedTracksUseCase>();
        services.AddScoped<GetTracksUseCase>();
        services.AddScoped<GetTrackByGuidUseCase>();
        services.AddScoped<CreateTrackUseCase>();
        services.AddScoped<UpdateTrackUseCase>();
        services.AddScoped<DeleteTrackUseCase>();
        services.AddScoped<ITrackService, TrackService>();

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        var connectionString = configuration.GetConnectionString("MySqlConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString), "Mysql connection string cannot be null or empty.");
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        return services;
    }
}
