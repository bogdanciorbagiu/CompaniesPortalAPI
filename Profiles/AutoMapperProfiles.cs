using AutoMapper;
using CompaniesPortalAPI.DomainModels;
using CompaniesPortalAPI.Profiles.AfterMaps;
using DataModel = CompaniesPortalAPI.DataModels;

namespace CompaniesPortalAPI.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<DataModel.Student, Student>()
                .ReverseMap();

            CreateMap<DataModel.Gender, Gender>()
                .ReverseMap();

            CreateMap<DataModel.Address, Address>()
                .ReverseMap();

            CreateMap<UpdateStudentRequest, DataModel.Student>()
                .AfterMap<UpdateStudentRequestAftermap>();

            CreateMap<CreateStudentRequest, DataModel.Student>()
                .AfterMap<CreateStudentRequestAftermap>();
        }
    }
}
