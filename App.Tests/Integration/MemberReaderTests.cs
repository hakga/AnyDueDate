using App.Domain.Entities;
using App.Infrastructure;
using App.TestSupport;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace App.Tests.Integration;

public sealed class MemberReaderTests {
    [Fact]
    [Trait("Category", "Integration")]
    public async Task 読み取り結果を別コンテキストから取得でき追跡されない() {
        await using var database = await SqliteTestDatabase.CreateAsync();
        await using (var writeContext = database.CreateContext()) {
            writeContext.Members.Add(new Member {
                Name = "保存済みメンバー",
                Description = "SQLiteから読み取る"
            });
            await writeContext.SaveChangesAsync();
        }

        await using var readContext = database.CreateContext();
        var member = await new DBMemberReader(readContext).Load().SingleAsync();

        member.Name.Should().Be("保存済みメンバー");
        readContext.Entry(member).State.Should().Be(EntityState.Detached);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task SQLiteインメモリデータベースはインスタンスごとに分離される() {
        await using var firstDatabase = await SqliteTestDatabase.CreateAsync();
        await using var secondDatabase = await SqliteTestDatabase.CreateAsync();
        await using (var context = firstDatabase.CreateContext()) {
            context.Members.Add(new Member { Name = "分離確認" });
            await context.SaveChangesAsync();
        }

        await using var secondContext = secondDatabase.CreateContext();

        (await secondContext.Members.CountAsync()).Should().Be(0);
    }
}
