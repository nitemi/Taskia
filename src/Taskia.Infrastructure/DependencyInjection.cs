using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taskia.Application.Common.Interfaces;
using Taskia.Infrastructure.Persistence;
using Taskia.Infrastructure.Persistence.Interceptors;
using Taskia.Infrastructure.Services;

namespace Taskia.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<AuditableEntityInterceptor>();

        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? "Host=localhost;Database=taskia_db;Username=postgres;Password=postgres";

        services.AddDbContext<TaskiaDbContext>((sp, options) =>
        {
            var auditableInterceptor = sp.GetRequiredService<AuditableEntityInterceptor>();

            options.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(TaskiaDbContext).Assembly.FullName))
                   .AddInterceptors(auditableInterceptor);
        });

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<TaskiaDbContext>());

        return services;
    }
}
