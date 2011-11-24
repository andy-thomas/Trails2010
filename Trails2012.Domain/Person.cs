using System;
using System.ComponentModel.DataAnnotations;

namespace Trails2012.Domain
{
    public class Person : EntityBase
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]        
        public virtual string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public virtual string LastName { get; set; }

        [DisplayFormat(DataFormatString = @"{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Birth")]
        [Range(typeof(DateTime), "1/1/1900", "1/1/2050")]
        public virtual DateTime? DateOfBirth { get; set; }

        [StringLength(1)]
        [RegularExpression(@"^M|F$")]
        public virtual string Gender { get; set; } // TODO change to enum - not yet fully supported in EF
        
        public virtual string FullName { get; private set; }

        public virtual string GenderDesc
        {
            get
            {
                return Gender.Equals('M') ? "Male" : "Female";
            }
        }
    
    }
}
