using System.ComponentModel.DataAnnotations;

namespace Trails2012.Domain
{
    public class TransportType : EntityBase
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Transport Type")]
        public virtual string TransportTypeName { get; set; }        
        
        public virtual string Notes { get; set; }
    }
}
