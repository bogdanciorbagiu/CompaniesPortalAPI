using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesPortalAPI.DomainModels
{
    public class UpdateStudentRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid GenderId { get; set; }
        public string PostalAddress { get; set; }

    }
}
