using System.Collections.Generic;
using Trails2012.Domain;

namespace Trails2012.Models
{
    public class PersonModel
    {
        public Person Person { get; set; }
        public IEnumerable<Person> Persons { get; set; }

    }
}