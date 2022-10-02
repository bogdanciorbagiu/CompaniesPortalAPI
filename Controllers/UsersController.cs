using CompaniesPortalAPI.Helpers;
using CompaniesPortalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;
using System.Text.RegularExpressions;
using CompaniesPortalAPI.DomainModels;
using CompaniesPortalAPI.DataModels;

namespace CompaniesPortalAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            // only allow admins to access other user records
            var currentUserId = Guid.Parse(User.Identity.Name);
            if (id != currentUserId && !User.IsInRole(Roles.Admin))
                return Forbid();

            var user = _userService.GetById(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteUser([FromRoute] string id)
        {
            var result = _userService.DeleteUser(id);
            return result ? Ok() : NotFound();
        }
        [HttpPost]
        [Route("Create")]
        public IActionResult CreateUser([FromBody] CreateUserRequest request)
        {
            var result = _userService.CreateUser(new User() 
            { 
                Id = Guid.NewGuid(), 
                FirstName = request.FirstName, 
                LastName = request.LastName,
                Username = request.Username,
                Password = request.Password,
                Role = Roles.User
            });

            return result ? Ok() : NotFound(); ;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("Test")]
        public IActionResult TestCompanyAPI()
        {
            string symbol_search = "https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords=ibm&apikey=FWQV1RTKVIWO7QRV";
            Uri symbol_searchUri = new Uri(symbol_search);

            string overview = "https://www.alphavantage.co/query?function=OVERVIEW&symbol=IBM&apikey=FWQV1RTKVIWO7QRV";
            Uri overviewUri = new Uri(overview);

            string currentStockPrice = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=IBM&apikey=FWQV1RTKVIWO7QRV";
            Uri priceUri = new Uri(currentStockPrice);

            using (WebClient client = new WebClient())
            {
                //var da = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                //dynamic json_data = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(client.DownloadString(symbol_searchUri));
                dynamic json_data_2 = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(client.DownloadString(overviewUri));
                var document = JsonDocument.Parse(client.DownloadString(overviewUri));

                var jsonString = client.DownloadString(overviewUri);
                var company = JsonSerializer.Deserialize<CompanyOverview>(jsonString);
                //dynamic json_data_3 = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(client.DownloadString(priceUri));

                //var jsonString = client.DownloadString(priceUri);

                //var document = JsonDocument.Parse(client.DownloadString(priceUri));
                //var main = document.RootElement.EnumerateObject().FirstOrDefault(p => p.Name == "Time Series (Daily)");
                //var second = main.Value.EnumerateObject().FirstOrDefault(p => p.Name == DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd"));
                //var price = second.Value.EnumerateObject().FirstOrDefault(p => p.Name == "1. open").Value.ToString();

                //var id = properties.FirstOrDefault(p => p.Name == "2022-09-29").Value.GetString(); 
                //var price = properties.FirstOrDefault(p => p.Name == "1.open").Value.GetString();

                //var result = Regex.Replace(jsonString, "\"(\\d+)(\\.)?(\\d+)?[a-z]?[\\.\\:]\\s", "\"", RegexOptions.Multiline);

                // -------------------------------------------------------------------------

                // do something with the json_data
            }

            return Ok();
        }
    }
}
