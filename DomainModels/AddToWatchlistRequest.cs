using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesPortalAPI.DomainModels
{
    public class AddToWatchlistRequest
    {
        public string UserId { get; set; }
        public string CompanyId { get; set; }
    }
}
