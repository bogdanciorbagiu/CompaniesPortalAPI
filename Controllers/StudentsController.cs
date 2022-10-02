using AutoMapper;
using CompaniesPortalAPI.DomainModels;
using CompaniesPortalAPI.Helpers;
using CompaniesPortalAPI.Repositories;
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
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        public StudentsController(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentRepository.GetStudentsAsync();

            return Ok(_mapper.Map<List<Student>>(students));
        }
        [HttpGet]
        [Route("[controller]/{id:guid}"), ActionName("GetStudentByIdAsync")]
        public async Task<IActionResult> GetStudentByIdAsync([FromRoute] Guid id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            return student != null ? Ok(_mapper.Map<Student>(student)) : NotFound();
        }

        [HttpPut]
        [Route("[controller]/{id:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid id, [FromBody] UpdateStudentRequest request)
        {
            if (await _studentRepository.Exists(id)) 
            {
                var updatedStudent = await _studentRepository.UpdateStudent(id, _mapper.Map<DataModels.Student>(request));

                return updatedStudent != null ? Ok(_mapper.Map<Student>(updatedStudent)) : NotFound();
            }
            else
            {
                return NotFound();
            }
            ;

        }

        [HttpDelete]
        [Route("[controller]/{id:guid}")]
        public async Task<IActionResult> DeleteStudentAsync([FromRoute] Guid id)
        {
            if (await _studentRepository.Exists(id))
            {
                var result = await _studentRepository.DeleteStudent(id);
                return Ok(_mapper.Map<Student>(result));
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        [Route("[controller]/Create")]
        public async Task<IActionResult> CreateStudentAsync([FromBody] CreateStudentRequest request)
        {
            var student = await _studentRepository.CreateStudentAsync(_mapper.Map<DataModels.Student>(request));

            return CreatedAtAction(nameof(GetStudentByIdAsync), new { id = student.Id}, _mapper.Map<Student>(student));
        }
    }
}
