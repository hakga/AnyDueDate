using System.ComponentModel.DataAnnotations;

namespace App.Domain.Entities {
    public class Category {
        public int Id { get; set; }
        [MaxLength(20)]
        [Display(Name = "カテゴリー名")]
        public string? Name { get; set; }
        [Display(Name = "カテゴリー番号")]
        public short CategoryCode { get; set; }
        [MaxLength(100)]
        [Display(Name = "カテゴリー説明")]
        public string? Description { get; set; }
    }
}
