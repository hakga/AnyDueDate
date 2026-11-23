using App.DB;
using App.Domain.Entities;
using App.Domain.Entities.Applications;
using Microsoft.EntityFrameworkCore;

namespace App.Extensions {
    /// <summary>期限データ（基本データ・応用データ）向けの汎用クエリ／操作。</summary>
    public static class DueDateQueryExtensions {
        /// <summary>未変更かつ未破棄の DueDateUnit（各系列の最新版）。</summary>
        public static IQueryable<DueDateUnit> WhereCurrent(this IQueryable<DueDateUnit> units) =>
            units.Where(u => u.ReplacedById == null && u.DiscardedAt == null);

        /// <summary>指定カテゴリーの応用データと、その最新の期限・対象者を取得する。</summary>
        public static IQueryable<CurrentApplication<TApplication>> QueryCurrent<TApplication>(this SampleDbContext db)
            where TApplication : DueDateApplication =>
            from application in db.Set<TApplication>()
            from current in db.DueDateUnits.WhereCurrent()
            where current.Id == application.OriginUnitId || current.OriginId == application.OriginUnitId
            select new CurrentApplication<TApplication> {
                Application = application,
                Current = current,
                Target = current.TargetUnit
            };

        /// <summary>
        /// 原初と全ての変更版をまとめて論理削除する。対象が原初でない場合も系列全体を破棄する。
        /// </summary>
        /// <returns>更新した DueDateUnit の件数</returns>
        public static async Task<int> DiscardSeriesAsync(
            this SampleDbContext db, int dueDateUnitId, DateTime now, CancellationToken cancellationToken = default) {
            var originId = await db.DueDateUnits
                .Where(u => u.Id == dueDateUnitId)
                .Select(u => (int?)(u.OriginId ?? u.Id))
                .SingleOrDefaultAsync(cancellationToken)
                ?? throw new KeyNotFoundException($"DueDateUnit {dueDateUnitId} が見つかりません");

            return await db.DueDateUnits
                .Where(u => (u.Id == originId || u.OriginId == originId) && u.DiscardedAt == null)
                .ExecuteUpdateAsync(s => s.SetProperty(u => u.DiscardedAt, now), cancellationToken);
        }
    }
}
