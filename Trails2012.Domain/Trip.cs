using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Trails2012.Domain
{
    public class Trip : EntityBase
    {
        [Required(ErrorMessage = "The trip date is required", AllowEmptyStrings = false)]
        [DisplayFormat(DataFormatString = @"{0:d}", ApplyFormatInEditMode = true)]
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

        [Display(Name = "People Summary")]
        public virtual string PersonsSummary
        {
            get { 
                StringBuilder summary = new StringBuilder();
                if(Persons != null)
                    foreach (var person in Persons)
                    {
                        summary.Append(person.FirstName).Append(";");
                    }
                return summary.ToString();
            }
        }

    }

}
