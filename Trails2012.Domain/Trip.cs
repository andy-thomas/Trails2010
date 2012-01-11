using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Trails2012.Domain
{
    public class Trip : EntityBase
    {
        [Required(ErrorMessage = "The trip date is required", AllowEmptyStrings = false)]
        public virtual DateTime Date { get; set; }

        [StringLength(50)]
        public virtual string Weather { get; set; }

        [Display(Name = "Time Taken (Hours)")]
        public virtual decimal? TimeTaken { get; set; }
        
        public virtual string Notes { get; set; }

        [Required(ErrorMessage = "The trail is required")]
        public virtual int TrailId { get; set; }
        public virtual Trail Trail { get; set; }

        [Required(ErrorMessage = "The transport type is required")]
        public virtual int TransportTypeId { get; set; }
        public virtual TransportType TransportType { get; set; }

        [Display(Name = "People")]        
        public virtual ICollection<Person> Persons { get; set; }
    }

}
