using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Endpoint.Extensions;
using Endpoint.SignalRHub;
using Shared.Config;
using Domain.Enums;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

var configBuilder = builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
    configBuilder.AddJsonFile("appsettings.json", reloadOnChange: true, optional: false);

var config = configBuilder.Build();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.Configure<JwtSettings>(config.GetSection("JwtSettings"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? throw new InvalidOperationException("JwtSettings configuration section is missing or invalid.");

    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey)),
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            context.Response.StatusCode = 401;
            context.Response.WriteAsync(JsonSerializer.Serialize(new { message = context.Exception.Message }));
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminPolicy", policy => policy.RequireRole(Role.Admin.ToString()))
    .AddPolicy("UserPolicy", policy => policy.RequireRole(Role.User.ToString(), Role.Admin.ToString()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        var allowedOrigins = config.GetSection("AllowedOrigins").Get<string[]>();
        if (allowedOrigins != null)
        {
            _ = builder.WithOrigins(allowedOrigins)
                       .AllowAnyMethod()
                       .AllowAnyHeader();
        }
    });
    options.AddPolicy("AllowAnyOrigin", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(config.GetSection("RateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(config.GetSection("RateLimitingPolicies"));
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ISharedDb, SharedDb>();
builder.Services.AddSignalR();

AppServiceCollectionExtensions.AddInfrastructure(builder.Services, config);
AppServiceCollectionExtensions.AddApplicationServices(builder.Services, config);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/root/.aspnet/DataProtection-Keys"))
    .SetApplicationName("MyApp");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseHttpsRedirection();
}

Console.WriteLine($"Cors Policy: {config.GetSection("CorsPolicy").Value}");
app.UseCors(config.GetSection("CorsPolicy").Value ?? "AllowAnyOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
app.MapHub<VocabHub>("/vocabhub");

app.Run();
