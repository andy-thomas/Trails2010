using System;

namespace Trails2012.Domain
{
    public class Person : EntityBase
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime? DateOfBirth { get; set; }  
        public virtual char Gender { get; set; } // TODO change to enum
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
