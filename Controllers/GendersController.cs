using AutoMapper;
using CompaniesPortalAPI.DomainModels;
using CompaniesPortalAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesPortalAPI.Controllers
{
    [ApiController]
    public class GendersController : Controller
    {
        private readonly IGenderRepository _genderRepository;
        private readonly IMapper _mapper;
        public GendersController(IGenderRepository genderRepository, IMapper mapper)
        {
            _genderRepository = genderRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllGenders()
        {
            var genders = await _genderRepository.GetGendersAsync();
            return Ok(_mapper.Map<List<Gender>>(genders));
        }
    }
}
