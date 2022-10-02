using CompaniesPortalAPI.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesPortalAPI.Repositories
{
    public interface IGenderRepository
    {
        Task<List<Gender>> GetGendersAsync();
    }
}
