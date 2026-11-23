using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities.Applications {
    /// <summary>
    /// 応用データ（カテゴリー別機能用の拡張データ）の基底。
    /// ※ここでの "Application" はASP.NETのアプリケーションや「申請」ではなく「応用データ」を指す。
    /// 原初の <see cref="DueDateUnit"/> と主キーを共有し（1:0..1）、期限変更後の版とも共有される。
    /// 派生クラスはカテゴリーごとに独立したテーブル（TPC）にマップされる。
    /// </summary>
    public abstract class DueDateApplication {
        /// <summary>原初 DueDateUnit の Id（期限変更後も不変）</summary>
        [Key]
        public int OriginUnitId { get; private set; }
        public DueDateUnit OriginUnit { get; private set; } = null!;

        /// <summary>この応用データ型が対応するカテゴリー番号（型ごとに固定・DB非マップ）</summary>
        [NotMapped]
        public abstract short CategoryCode { get; }

        [Display(Name = "登録日")]
        public DateTime CreatedAt { get; private set; }

        /// <summary>EF Core 用</summary>
        protected DueDateApplication() { }

        protected DueDateApplication(DueDateUnit origin, DateTime now) {
            ArgumentNullException.ThrowIfNull(origin);
            if (origin.OriginId is not null || origin.ReplacedById is not null)
                throw new InvalidOperationException("応用データは原初の DueDateUnit にのみ登録できます");
            if (origin.DiscardedAt is not null)
                throw new InvalidOperationException("破棄済みの DueDateUnit には応用データを登録できません");
            if (origin.CategoryCode != CategoryCode)
                throw new InvalidOperationException(
                    $"{GetType().Name} はカテゴリー {CategoryCode} 用です（指定: {origin.CategoryCode}）");

            OriginUnit = origin;   // OriginUnitId は保存時にナビゲーションから設定される
            CreatedAt = now;
        }
    }
}
