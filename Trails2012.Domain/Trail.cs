using System.ComponentModel.DataAnnotations;

namespace Trails2012.Domain
{
    public class Trail : EntityBase
    {
        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }

        public virtual Location Location { get; set; }
        public virtual TrailType TrailType { get; set; }

        public virtual string Description { get; set; }
        public virtual float Distance { get; set; }

        [Display(Name = "Elevation Gain")]
        public virtual float ElevationGain { get; set; }

        [DisplayFormat(DataFormatString = @"{0:c}", ApplyFormatInEditMode = true)]
        public virtual float Cost { get; set; }

        [Display(Name = "Estimated Time (Hours)")]
        public virtual float EstimatedTime { get; set; }

        [Display(Name = "Is Loop)")]
        public virtual bool IsLoop { get; set; }

        [Display(Name = "Difficulty Level)")]
        public virtual Difficulty Difficulty { get; set; }

        [Display(Name = "Return On Cost Rating (1-10)")]
        [Range(1F, 10F)]
        public virtual float ReturnOnCost { get; set; }

        [Display(Name = "Return On Effort Rating (1-10)")]
        [Range(1F, 10F)]
        public virtual float ReturnOnEffort { get; set; }

        [Display(Name = "Overall Rating (1-10)")]
        [Range(1F, 10F)]
        public virtual float OverallGrade { get; set; }

        public virtual string Notes { get; set; }
    }
}
