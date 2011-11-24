using System.ComponentModel.DataAnnotations;

namespace Trails2012.Domain
{
    public class Region : EntityBase
    {
        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }
    }
}
