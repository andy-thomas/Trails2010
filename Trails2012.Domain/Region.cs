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
        public virtual int LocationCount
        {
            get
            {
                int count = 0;
                if (Locations != null && Locations.Count > 0)
                    count = Locations.Count;
                return count;
            }
            private set { }
        }

        // Andy - I have put this in to make sure that the test that uses VerifyTheMappings 
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Region region = obj as Region;
            if (region == null) return false;

            if (!Id.Equals(region.Id)) return false;
            if ((Name != null && !Name.Equals(region.Name)) ||
                (Name == null && region.Name != null)) return false;
            if ((Description != null && !Description.Equals(region.Description)) ||
                (Description == null && region.Description != null)) return false;
  
            return true;

        }
    }
}
