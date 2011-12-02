using System.ComponentModel.DataAnnotations;

namespace Trails2012.Domain
{
    public class Trail : EntityBase
    {
        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }

        public virtual int LocationId { get; set; }
        public virtual Location Location { get; set; }

        public virtual int TrailTypeId { get; set; }
        public virtual TrailType TrailType { get; set; }

        public virtual string Description { get; set; }
        public virtual decimal? Distance { get; set; }

        [Display(Name = "Elevation Gain")]
        public virtual decimal? ElevationGain { get; set; }

        [DisplayFormat(DataFormatString = @"{0:c}", ApplyFormatInEditMode = true)]
        public virtual decimal? Cost { get; set; }

        [Display(Name = "Estimated Time (Hours)")]
        public virtual decimal? EstimatedTime { get; set; }

        [Display(Name = "Is Loop)")]
        public virtual bool IsLoop { get; set; }

        [Display(Name = "Difficulty Level)")]
        public virtual int? DifficultyId { get; set; }
        public virtual Difficulty Difficulty { get; set; }

        [Display(Name = "Return On Cost Rating (1-10)")]
        [Range(1, 10)]
        public virtual decimal? ReturnOnCost { get; set; }

        [Display(Name = "Return On Effort Rating (1-10)")]
        [Range(1, 10)]
        public virtual decimal? ReturnOnEffort { get; set; }

        [Display(Name = "Overall Rating (1-10)")]
        [Range(1, 10)]
        public virtual decimal? OverallGrade { get; set; }

        public virtual string Notes { get; set; }
    }
}
