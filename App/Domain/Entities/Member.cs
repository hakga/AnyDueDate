using System.ComponentModel.DataAnnotations;

namespace App.Domain.Entities {
    public class Member {
        public int Id { get; set; }
        [Display(Name = "氏名")]
        public string? Name { get; set; }
        [Display(Name = "説明")]
        public string? Description { get; set; }
    }
}
