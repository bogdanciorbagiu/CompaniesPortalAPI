using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesPortalAPI.DataModels
{
    public class CompaniesContext : DbContext
    {
        public CompaniesContext(DbContextOptions<CompaniesContext> options): base(options)
        {

        }

        public DbSet<Student> Student { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Address> Address { get; set; }

        public DbSet<User> User { get; set; }
        public DbSet<UserCompany> UserCompany { get; set; }
    }
}
