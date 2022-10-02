using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesPortalAPI.DataModels
{
    public class UserCompany
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string CompanySymbol { get; set; }
        public string CurrentStockPrice { get; set; }

        public User User { get; set; }
    }
}
