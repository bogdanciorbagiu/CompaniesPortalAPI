using CompaniesPortalAPI.DataModels;
using CompaniesPortalAPI.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace CompaniesPortalAPI.Services
{
    public class UserService : IUserService
    {
        private readonly CompaniesContext _context;
        private readonly AppSettings _appSettings;
        public UserService(IOptions<AppSettings> appSettings, CompaniesContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }

        public User Authenticate(string username, string password)
        {
            var user = _context.User.SingleOrDefault(x => x.Username == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user.WithoutPassword();
        }

        public IEnumerable<User> GetAll()
        {
            return _context.User.WithoutPasswords();
        }

        public User GetById(Guid id)
        {
            var user = _context.User.FirstOrDefault(x => x.Id == id);
            return user.WithoutPassword();
        }

        public bool DeleteUser(string id)
        {
            var user = _context.User.FirstOrDefault(u => u.Id == Guid.Parse(id));
            if (user != null && user.Role != Roles.Admin)
            {
                var companies = _context.UserCompany.Where(u => u.UserId == Guid.Parse(id)).ToList();

                foreach (var company in companies)
                {
                    _context.UserCompany.Remove(company);
                }

                _context.User.Remove(user);
                _context.SaveChanges();
                return true;
            }
            
            return false;
        }

        public bool CreateUser(User userToCreate)
        {
            _context.User.Add(userToCreate);
            var result = _context.SaveChanges();
            return result != 0;

        }
    }
}
