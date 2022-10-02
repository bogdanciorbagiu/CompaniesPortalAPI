using CompaniesPortalAPI.DataModels;
using CompaniesPortalAPI.DomainModels;
using CompaniesPortalAPI.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CompaniesPortalAPI.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly CompaniesContext _context;
        private readonly AppSettings _appSettings;
        public CompanyService(IOptions<AppSettings> appSettings, CompaniesContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }

        public List<Company> GetCompanies(string id)
        {
            string symbol_search = "https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords=" + id + "&apikey=" + _appSettings.APIKey;
            Uri symbol_searchUri = new Uri(symbol_search);
            using (WebClient client = new WebClient())
            {
                var jsonString = client.DownloadString(symbol_searchUri);
                var result = Regex.Replace(jsonString, "\"(\\d+)(\\.)?(\\d+)?[a-z]?[\\.\\:]\\s", "\"", RegexOptions.Multiline);

                var company = JsonSerializer.Deserialize<BestMatches>(result);
                return company.bestMatches;
            }

        }

        public List<UserCompany> GetCompaniesByUser(Guid id)
        {
            var companies = _context.UserCompany.Where(u => u.UserId == id);
            if (companies == null)
            {
                return null;
            }
            else
            {
                using (WebClient client = new WebClient())
                {
                    foreach (var company in companies)
                    {
                        string currentStockPrice = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=" + company.CompanySymbol + "&apikey=" + _appSettings.APIKey;
                        Uri priceUri = new Uri(currentStockPrice);
                        var document = JsonDocument.Parse(client.DownloadString(priceUri));
                        var main = document.RootElement.EnumerateObject().FirstOrDefault(p => p.Name == "Time Series (Daily)");
                        if (main.Value.ToString() == "")
                        {
                            company.CurrentStockPrice = "";
                            continue;
                        }
                        var second = main.Value.EnumerateObject().FirstOrDefault(p => p.Name == DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd"));
                        if (second.Value.ToString() == "")
                        {
                            var firstDate = Regex.Match(main.Value.ToString(), "[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]").Value.Replace("{","").Replace("}", "");
                            if (String.IsNullOrEmpty(firstDate))
                            {
                                company.CurrentStockPrice = "";
                                continue;
                            }
                            else
                            {
                                second = main.Value.EnumerateObject().FirstOrDefault(p => p.Name == firstDate);
                                if (second.Value.ToString() == "")
                                {
                                    company.CurrentStockPrice = "";
                                    continue;
                                }
                            }
                        }
                        var price = second.Value.EnumerateObject().FirstOrDefault(p => p.Name == "1. open").Value.ToString();
                        company.CurrentStockPrice = price;
                    }
                }
                return companies.ToList();
            }
                
        }

        public bool AddCompanyToUser(Guid userId, string companySymbol)
        {
            if (_context.UserCompany.Count(u => u.UserId == userId) == 4)
            {
                return false;
            }
            var exists = _context.UserCompany.FirstOrDefault(u => u.UserId == userId && u.CompanySymbol == companySymbol);

            if (exists != null)
            {
                return false;
            }

            _context.UserCompany.Add(
                new UserCompany()
                {
                    Id = Guid.NewGuid(),
                    CompanySymbol = companySymbol,
                    CurrentStockPrice = "",
                    UserId = userId
                }
                );

            _context.SaveChanges();

            return true; 

        }

        public bool DeleteFromWatchlist(string symbol, string user)
        {
            var company = _context.UserCompany.FirstOrDefault(u => u.UserId == Guid.Parse(user) && u.CompanySymbol == symbol);

            if (company != null)
            {
                _context.UserCompany.Remove(company);
                _context.SaveChanges();
                return true;
            }
            return false;
            
        }

        public CompanyOverview GetCompanyOverview(string symbol)
        {
            using (WebClient client = new WebClient())
            {
                string overview = "https://www.alphavantage.co/query?function=OVERVIEW&symbol=" + symbol +"&apikey=" + _appSettings.APIKey;
                Uri overviewUri = new Uri(overview);
                var jsonString = client.DownloadString(overviewUri);
                var company = JsonSerializer.Deserialize<CompanyOverview>(jsonString);
                return company ?? new CompanyOverview();
            }
        }
    }

}
