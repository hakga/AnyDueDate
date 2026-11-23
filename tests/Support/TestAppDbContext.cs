using App.DB;
using Microsoft.EntityFrameworkCore;

namespace App.TestSupport;

/// <summary>本番モデルをSQLiteのインメモリDBで使うテスト用コンテキスト。</summary>
public sealed class TestAppDbContext(DbContextOptions<SampleDbContext> options)
    : SampleDbContext(options) {
}
