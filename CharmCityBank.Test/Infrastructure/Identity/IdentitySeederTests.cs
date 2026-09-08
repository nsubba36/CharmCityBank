using CharmCityBank.Api.Infrastructure.Identity;
using CharmCityBank.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CharmCityBank.Test.Infrastructure.Identity;

public class IdentitySeederTests
{
    [Fact]
    public async Task SeedRolesAsync_ShouldCreateAdminAndUsers()
    {
        await using var connection = new SqliteConnection("DataSource=:memory:");
        
        await connection.OpenAsync();
        var services = new ServiceCollection();
        
        services.AddLogging();

        services.AddDbContext<IdentityTestDbContext>(options => options.UseSqlite(connection));

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<IdentityTestDbContext>();
        
        await using var serviceProvider = services.BuildServiceProvider();
        await using var scope = serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IdentityTestDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        
        await IdentitySeeder.SeedRolesAsync(scope.ServiceProvider);
        
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        
        Assert.True(await roleManager.RoleExistsAsync(Roles.Admin));
        Assert.True(await roleManager.RoleExistsAsync(Roles.User));
    }

    private sealed class IdentityTestDbContext(
        DbContextOptions<IdentityTestDbContext> options
    ) : IdentityDbContext<ApplicationUser>(options);
}