using System.ComponentModel.DataAnnotations;

namespace Trails2012.Domain
{
    public class TrailType : EntityBase
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Trail Type")]
        public virtual string TrailTypeName { get; set; }
    }
}
