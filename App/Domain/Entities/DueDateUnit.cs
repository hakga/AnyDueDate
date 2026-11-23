using System.ComponentModel.DataAnnotations;

namespace App.Domain.Entities {
    public class DueDateUnit {
        [Key]
        public int Id { get; set; }
        [Display(Name = "期限日")]
        public DateTime DueDate { get; set; }
        [Display(Name = "対象者")]
        public TargetUnit TargetUnit { get; set; } = null!;
        [Display(Name = "カテゴリー番号")]
        public short CategoryCode { get; set; }
        [Display(Name = "期限変更")]
        public int? ReplacedById { get; set; }
        public DueDateUnit? ReplacedBy { get; set; }
        /// <summary>原初 DueDateUnit の Id。null の場合は自身が原初。</summary>
        [Display(Name = "原初期限")]
        public int? OriginId { get; set; }
        public DueDateUnit? Origin { get; set; }
        /// <summary>論理削除日時。破棄時は原初と全ての変更版に同じ値を設定する。</summary>
        [Display(Name = "破棄日")]
        public DateTime? DiscardedAt { get; set; }
        [Display(Name = "設置日")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 期限日だけを変更した新しい版を作り、自身から ReplacedBy でリンクする。
        /// 応用データは原初に紐づいたまま共有されるため複製しない。
        /// </summary>
        public DueDateUnit Replace(DateTime newDueDate, DateTime now) {
            if (ReplacedById is not null || ReplacedBy is not null)
                throw new InvalidOperationException("最新版以外の期限は変更できません");
            if (DiscardedAt is not null)
                throw new InvalidOperationException("破棄済みの期限は変更できません");

            var next = new DueDateUnit {
                DueDate = newDueDate,
                TargetUnit = TargetUnit,
                CategoryCode = CategoryCode,
                OriginId = OriginId ?? Id,
                CreatedAt = now
            };
            ReplacedBy = next;
            return next;
        }
    }
}
