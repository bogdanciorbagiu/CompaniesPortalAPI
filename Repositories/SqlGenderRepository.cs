using CompaniesPortalAPI.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesPortalAPI.Repositories
{
    public class SqlGenderRepository : IGenderRepository
    {
        private readonly CompaniesContext _context;
        public SqlGenderRepository(CompaniesContext context)
        {
            _context = context;
        }
        public async Task<List<Gender>> GetGendersAsync()
        {
            return await _context.Gender.ToListAsync();
        }
    }
}
