using System.ComponentModel.DataAnnotations;

namespace Trails2012.Domain
{
    public class Difficulty : EntityBase
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Difficulty Level")]
        public virtual string DifficultyType { get; set; }
    }
}
