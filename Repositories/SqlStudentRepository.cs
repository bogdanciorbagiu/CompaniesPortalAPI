using CompaniesPortalAPI.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesPortalAPI.Repositories
{
    public class SqlStudentRepository : IStudentRepository
    {
        private readonly CompaniesContext _context;
        public SqlStudentRepository(CompaniesContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _context.Student.Include("Gender").Include("Address").ToListAsync();
        }

        public async Task<Student> GetStudentByIdAsync(Guid id)
        {
            return await _context.Student.Include("Gender").Include("Address").FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _context.Student.AnyAsync(s => s.Id == id);
        }

        public async Task<Student> UpdateStudent(Guid id, Student student)
        {
            var existingStudent = await GetStudentByIdAsync(id);
            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.GenderId = student.GenderId;
            existingStudent.Address.PostalAddress = student.Address.PostalAddress;

            await _context.SaveChangesAsync();

            return existingStudent;
            
        }

        public async Task<Student> DeleteStudent(Guid id)
        {
            var student = await GetStudentByIdAsync(id);

            if (student != null)
            {
                _context.Student.Remove(student);
                await _context.SaveChangesAsync();
            }

            return null;
        }

        public async Task<Student> CreateStudentAsync(Student studentToCreate)
        {
            var student = await _context.Student.AddAsync(studentToCreate);

            await _context.SaveChangesAsync();

            return student.Entity;

        }

        
    }
}
