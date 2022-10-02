using AutoMapper;
using CompaniesPortalAPI.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesPortalAPI.Profiles.AfterMaps
{
    public class UpdateStudentRequestAftermap : IMappingAction<UpdateStudentRequest, DataModels.Student>
    {
        public void Process(UpdateStudentRequest source, DataModels.Student destination, ResolutionContext resolutionContext)
        {
            destination.Address = new DataModels.Address()
            {
                PostalAddress = source.PostalAddress
            };
        }
    }
}
