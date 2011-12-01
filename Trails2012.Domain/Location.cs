using System;
using System.ComponentModel.DataAnnotations;

namespace Trails2012.Domain
{
    public class Location : EntityBase
    {
        [NonSerialized]
        private Domain.Region _region;

        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }
        public virtual string Directions { get; set; }

        [StringLength(50)]
        [Display(Name = "Map Reference")]
        public virtual string MapReference { get; set; }

        public int RegionId { get; set; }
        public virtual Region Region
        {
            get { return _region; }
            set { _region = value; }
        }
    }
}
