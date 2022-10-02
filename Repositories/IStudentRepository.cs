using CompaniesPortalAPI.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaniesPortalAPI.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetStudentsAsync();
        Task<Student> GetStudentByIdAsync(Guid id);

        Task<bool> Exists(Guid id);

        Task<Student> UpdateStudent(Guid id, Student student);

        Task<Student> DeleteStudent(Guid id);

        Task<Student> CreateStudentAsync(Student student);
    }
}
