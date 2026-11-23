using App.Domain.Entities;
using App.TestSupport;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace App.E2ETests;

[Collection("Browser")]
public sealed class MembersPageTests(BrowserFixture fixture) : IAsyncLifetime {
    private IBrowserContext? _browserContext;
    private IPage? _page;

    public async Task InitializeAsync() {
        await ClearMembersAsync();
        await using var database = fixture.CreateDbContext();
        database.Members.Add(new Member {
            Name = "期限管理の対象者",
            Description = "画面表示の確認"
        });
        await database.SaveChangesAsync();

        _browserContext = await fixture.Browser.NewContextAsync(new BrowserNewContextOptions {
            BaseURL = fixture.BaseUrl
        });
        _page = await _browserContext.NewPageAsync();
    }

    [Fact]
    [Trait("Category", "E2E")]
    public async Task 一覧画面に保存済みメンバーを表示する() {
        await _page!.GotoAsync("/Home/Samples");

        await Expect(_page.GetByRole(AriaRole.Heading, new() { Name = "Samples" }))
            .ToBeVisibleAsync();
        await Expect(_page.GetByText("期限管理の対象者", new() { Exact = true }))
            .ToBeVisibleAsync();
    }

    public async Task DisposeAsync() {
        if (_browserContext is not null) {
            await _browserContext.CloseAsync();
            _browserContext = null;
        }

        await ClearMembersAsync();
    }

    private async Task ClearMembersAsync() {
        await using var database = fixture.CreateDbContext();
        database.Members.RemoveRange(database.Members);
        await database.SaveChangesAsync();
    }
}
