using Microsoft.EntityFrameworkCore;
using App.DB;
using App.Domain.Repositories;
using App.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SampleDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IMemberReader, DBMemberReader>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    // 【追加】開発環境：詳細なスタックトレース付きエラー画面を表示
    app.UseDeveloperExceptionPage();
} else {
    // 本番環境：ユーザー用のエラー画面へ遷移させ、セキュリティヘッダー(HSTS)を付与
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // 【追加】
}
// 【追加】HTTP アクセスを自動的に HTTPS に転送
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute( name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
