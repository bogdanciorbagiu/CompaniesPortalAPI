using CompaniesPortalAPI.DataModels;
using CompaniesPortalAPI.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesPortalAPI.Services
{
    public interface ICompanyService
    {
        List<Company> GetCompanies(string id);
        List<UserCompany> GetCompaniesByUser(Guid id);
        bool AddCompanyToUser(Guid userId, string companySymbol);
        bool DeleteFromWatchlist(string symbol, string user);
        CompanyOverview GetCompanyOverview(string symbol);
    }
}
