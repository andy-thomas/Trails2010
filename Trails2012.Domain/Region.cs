using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace Trails2012.Domain
{
    public class Region : EntityBase
    {
        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        [ScriptIgnore] // This helps suppress some circular reference errors when using EF and the Telerik grid
        public virtual ICollection<Location> Locations { get; set; }
        public int LocationCount
        {
            get
            {
                int count = 0;
                if (Locations != null && Locations.Count > 0)
                    count = Locations.Count;
                return count;
            }
        }
    }
}
