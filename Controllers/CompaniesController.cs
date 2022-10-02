using CompaniesPortalAPI.DomainModels;
using CompaniesPortalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesPortalAPI.Controllers
{
    [Authorize]
    [ApiController]
    public class CompaniesController : Controller
    {
        private readonly ICompanyService _companyService;
        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        [HttpGet]
        [Route("[controller]/{id}")]
        public IActionResult GetCompanies([FromRoute] string id)
        {
            var companies = _companyService.GetCompanies(id);
            return companies != null ? Ok(companies) : NotFound();
        }
        [HttpGet]
        [Route("[controller]/Watchlist/{id}")]
        public IActionResult GetCompaniesByUser([FromRoute] string id)
        {
            var companies = _companyService.GetCompaniesByUser(Guid.Parse(id));
            return companies != null && companies.Count > 0 ? Ok(companies) : NotFound();
        }
        [HttpGet]
        [Route("[controller]/Overview/{id}")]
        public IActionResult GetCompanyOverview([FromRoute] string id)
        {
            var companies = _companyService.GetCompanyOverview(id);
            return companies != null ? Ok(companies) : NotFound();
        }
        [HttpPost]
        [Route("[controller]/Create")]
        public IActionResult AddToWatchlist([FromBody] AddToWatchlistRequest addToWatchlistRequest)
        {
            var result = _companyService.AddCompanyToUser(Guid.Parse(addToWatchlistRequest.UserId), addToWatchlistRequest.CompanyId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("[controller]/Watchlist/{symbol}/{user}")]
        public IActionResult DeleteFromWatchlist([FromRoute] string symbol, string user)
        {

            var result = _companyService.DeleteFromWatchlist(symbol, user);
            return result ? Ok() : NotFound();
        }
    }
}
