using AutoMapper;
using CompaniesPortalAPI.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesPortalAPI.Profiles.AfterMaps
{
    public class CreateStudentRequestAftermap : IMappingAction<CreateStudentRequest, DataModels.Student>
    {
        public void Process(CreateStudentRequest source, DataModels.Student destination, ResolutionContext resolutionContext)
        {
            destination.Id = Guid.NewGuid();

            destination.Address = new DataModels.Address()
            {
                Id = Guid.NewGuid(),
                PostalAddress = source.PostalAddress
            };

        }
    }
}
