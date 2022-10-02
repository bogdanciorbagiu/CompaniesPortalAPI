using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesPortalAPI.DomainModels
{
    public class Student
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Guid GenderId { get; set; }
        public Address Address { get; set; }

        public Gender Gender { get; set; }
    }
}
