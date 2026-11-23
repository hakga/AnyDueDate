using App.Domain.Entities;
using App.Domain.Entities.Applications;
using App.Extensions;
using App.TestSupport;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace App.Tests.Integration;

/// <summary>テスト専用カテゴリーの応用データ。</summary>
public sealed class TestCategoryApplication : DueDateApplication {
    public const short Code = 99;
    public override short CategoryCode => Code;
    public string Note { get; set; } = null!;

    private TestCategoryApplication() { }
    public TestCategoryApplication(DueDateUnit origin, DateTime now, string note) : base(origin, now) => Note = note;
}

public sealed class DueDateApplicationTests {
    private static readonly DateTime Now = new(2026, 11, 23);

    private static DueDateUnit NewOrigin(short categoryCode = TestCategoryApplication.Code) => new() {
        DueDate = new DateTime(2026, 12, 31),
        TargetUnit = new TargetUnit { pandaid = "P000000001", TargetName = "対象者" },
        CategoryCode = categoryCode,
        CreatedAt = Now
    };

    private static async Task<int> SeedAsync(SqliteTestDatabase database) {
        await using var context = database.CreateContext();
        var origin = NewOrigin();
        context.Add(new TestCategoryApplication(origin, Now, "拡張"));
        await context.SaveChangesAsync();
        return origin.Id;
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void カテゴリー番号が一致しない場合は登録できない() {
        var act = () => new TestCategoryApplication(NewOrigin(categoryCode: 1), Now, "x");

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task 期限変更後も応用データは原初に紐づいたまま最新版と組で取得できる() {
        await using var database = await SqliteTestDatabase.CreateAsync();
        var originId = await SeedAsync(database);

        await using (var context = database.CreateContext()) {
            var origin = await context.DueDateUnits.Include(u => u.TargetUnit).SingleAsync(u => u.Id == originId);
            var second = origin.Replace(new DateTime(2027, 1, 31), Now);
            await context.SaveChangesAsync();
            second.Replace(new DateTime(2027, 2, 28), Now);
            await context.SaveChangesAsync();
        }

        await using var readContext = database.CreateContext();
        var current = await readContext.QueryCurrent<TestCategoryApplication>().SingleAsync();

        current.Application.OriginUnitId.Should().Be(originId);
        current.Application.Note.Should().Be("拡張");
        current.Current.DueDate.Should().Be(new DateTime(2027, 2, 28));
        current.Current.OriginId.Should().Be(originId);
        current.Target.pandaid.Should().Be("P000000001");
        (await readContext.Set<TestCategoryApplication>().CountAsync()).Should().Be(1);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task 系列を破棄すると原初と全変更版が論理削除され最新版として取得されない() {
        await using var database = await SqliteTestDatabase.CreateAsync();
        var originId = await SeedAsync(database);
        int latestId;
        await using (var context = database.CreateContext()) {
            var origin = await context.DueDateUnits.Include(u => u.TargetUnit).SingleAsync(u => u.Id == originId);
            var latest = origin.Replace(new DateTime(2027, 1, 31), Now);
            await context.SaveChangesAsync();
            latestId = latest.Id;
        }

        await using var discardContext = database.CreateContext();
        var updated = await discardContext.DiscardSeriesAsync(latestId, Now);

        updated.Should().Be(2);
        (await discardContext.DueDateUnits.CountAsync(u => u.DiscardedAt == Now)).Should().Be(2);
        (await discardContext.QueryCurrent<TestCategoryApplication>().AnyAsync()).Should().BeFalse();
        (await discardContext.Set<TestCategoryApplication>().CountAsync()).Should().Be(1, "応用データ自体は履歴として残る");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void 変更済みの版は再変更できない() {
        var origin = NewOrigin();
        origin.Replace(new DateTime(2027, 1, 31), Now);

        var act = () => origin.Replace(new DateTime(2027, 2, 28), Now);

        act.Should().Throw<InvalidOperationException>();
    }
}
