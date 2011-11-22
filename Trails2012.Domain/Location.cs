namespace Trails2012.Domain
{
    public class Location : EntityBase
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual Region Region { get; set; }
        public virtual string Directions { get; set; }
        public virtual string MapReference { get; set; }
    }
}
