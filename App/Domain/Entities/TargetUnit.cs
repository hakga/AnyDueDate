using System.ComponentModel.DataAnnotations;

namespace App.Domain.Entities {
    public class TargetUnit {
        [Key]
        public int Id { get; set; }
        [Display(Name = "PANDAID")]
        [MaxLength(10)]
        public string pandaid { get; set; } = null!;
        [Display(Name = "氏名")]
        [MaxLength(25)]
        public string? TargetName { get; set; }
        [Display(Name = "所属")]
        [MaxLength(50)]
        public string? BelongedTo { get; set; }
    }
}
