using System.ComponentModel.DataAnnotations;

namespace Trails2012.Domain
{
    public class Trail : EntityBase
    {
        [Required(ErrorMessage = "A trail name is required", AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string Name { get; set; }

        public virtual int LocationId { get; set; }
        public virtual Location Location { get; set; }

        public virtual int TrailTypeId { get; set; }
        public virtual TrailType TrailType { get; set; }

        //[StringLength(1000)]
        public virtual string Description { get; set; }
        public virtual decimal? Distance { get; set; }

        [Display(Name = "Elevation Gain")]
        public virtual decimal? ElevationGain { get; set; }

        [DisplayFormat(DataFormatString = @"{0:c}", ApplyFormatInEditMode = false)]
        public virtual decimal? Cost { get; set; }

        [Display(Name = "Estimated Time (Hours)")]
        public virtual decimal? EstimatedTime { get; set; }

        [Display(Name = "Is Loop")]
        public virtual bool IsLoop { get; set; }

        [Display(Name = "Difficulty Level)")]
        public virtual int? DifficultyId { get; set; }
        public virtual Difficulty Difficulty { get; set; }

        [Display(Name = "Return On Cost Rating (1-10)")]
        [Range(1, 10, ErrorMessage = "Please provide a rating between 1 and 10")]
        public virtual decimal? ReturnOnCost { get; set; }

        [Display(Name = "Return On Effort Rating (1-10)")]
        [Range(1, 10, ErrorMessage = "Please provide a rating between 1 and 10")]
        public virtual decimal? ReturnOnEffort { get; set; }

        [Display(Name = "Overall Rating (1-10)")]
        [Range(1, 10, ErrorMessage = "Please provide a rating between 1 and 10")]
        public virtual decimal? OverallGrade { get; set; }

        public virtual string Notes { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        // I considered using the SQL Server Geography type, but it involved some extra plumbing in order to not couple the domain to Microsoft-specific data type
        // and anyway, it has been removed from EF v4.2 - won't be available until v4.5. Keep simple with Lat & Long coordinates for now
    }
}
