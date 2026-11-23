using App.Controllers;
using App.DB;
using App.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.TestSupport;

public static class TestApplication {
    public static WebApplication Create(SqliteConnection connection) {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions {
            ApplicationName = typeof(HomeController).Assembly.GetName().Name,
            ContentRootPath = FindAppDirectory(),
            EnvironmentName = "Testing",
            Args = []
        });

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Services.AddDataProtection().UseEphemeralDataProtectionProvider();
        builder.WebHost.UseUrls("http://127.0.0.1:0");

        return AppBootstrap.Build(builder, services =>
            services.AddScoped<SampleDbContext>(_ => new TestAppDbContext(
                new DbContextOptionsBuilder<SampleDbContext>()
                    .UseSqlite(connection)
                    .Options)));
    }

    private static string FindAppDirectory() {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory);
             directory is not null;
             directory = directory.Parent) {
            var path = Path.Combine(directory.FullName, "App");
            if (File.Exists(Path.Combine(path, "App.csproj"))) {
                return path;
            }
        }

        throw new DirectoryNotFoundException("テスト対象のApp/App.csprojが見つかりません。");
    }
}
