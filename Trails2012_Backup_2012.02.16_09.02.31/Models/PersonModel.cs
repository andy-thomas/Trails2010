using System.Collections.Generic;
using System.ComponentModel.Composition;
using Trails2012.DataAccess;
using Trails2012.Domain;

namespace Trails2012.Models
{
    public class PersonModel
    {
        [Import]
        private IRepository Repository { get; set; }

        public Person Person { get; set; }
        public IEnumerable<Person> Persons { get; set; }
        public Dictionary<string, string> GenderList { get; set; }

        public PersonModel()
        {
            GenderList = PopulateGenderList();
        }

        private Dictionary<string, string> PopulateGenderList()
        {
            return new Dictionary<string, string> {{"M", "Male"}, {"F", "Female"}};
        }

        public PersonModel(int id) : this()
        {
            Person = Repository.GetById<Person>(id);
        }
    }
}