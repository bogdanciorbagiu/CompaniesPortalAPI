using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesPortalAPI.DomainModels
{
    public class Company
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string region { get; set; }
        public string marketOpen { get; set; }
        public string marketClose { get; set; }
        public string timeZone { get; set; }
        public string currency { get; set; }
        public string matchScore { get; set; }
    }
}
