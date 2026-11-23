using App.DB;
using App.Domain.Repositories;
using App.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace App.Hosting {
    public static class AppBootstrap {
        public static WebApplication Build(
            WebApplicationBuilder builder,
            Action<IServiceCollection>? configureDatabase = null) {
            builder.Services.AddControllersWithViews();

            if (configureDatabase is null) {
                builder.Services.AddDbContext<SampleDbContext>(options =>
                    options.UseSqlServer(
                        builder.Configuration.GetConnectionString("DefaultConnection")));
            } else {
                configureDatabase(builder.Services);
            }

            builder.Services.AddScoped<IMemberReader, DBMemberReader>();

            var app = builder.Build();
            if (app.Environment.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                if (!app.Environment.IsEnvironment("Testing")) {
                    app.UseHsts();
                }
            }

            if (!app.Environment.IsEnvironment("Testing")) {
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            return app;
        }
    }
}

