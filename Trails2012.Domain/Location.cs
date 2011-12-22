using System;
using System.ComponentModel.DataAnnotations;

namespace Trails2012.Domain
{
    public class Location : EntityBase
    {
        //[NonSerialized]
        //private Region _region;

        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }
        public virtual string Directions { get; set; }

        [StringLength(50)]
        [Display(Name = "Map Reference")]
        public virtual string MapReference { get; set; }

        [Display(Name = "Region")]
        public virtual int RegionId { get; set; }
        public virtual Region Region { get; set; }
        //{
        //    get { return _region; }
        //    set { _region = value; }
        //}
    }
}
