namespace Trails2012.Domain
{
    public class Trail : EntityBase
    {
        public virtual string Name { get; set; }
        public virtual Location Location { get; set; }
        public virtual TrailType TrailType { get; set; }
        public virtual string Description { get; set; }
        public virtual float Distance { get; set; }
        public virtual float ElevationGain { get; set; }
        public virtual float Cost { get; set; }
        public virtual float EstimatedTime { get; set; }
        public virtual bool IsLoop { get; set; }
        public virtual Difficulty Difficulty { get; set; }
        public virtual float ReturnOnCost { get; set; }
        public virtual float ReturnOnEffort { get; set; }
        public virtual float OverallGrade { get; set; }
        public virtual string Notes { get; set; }
    }
}
