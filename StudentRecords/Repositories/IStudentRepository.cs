using System;
using System.Collections.Generic;
using System.Text;
using StudentRecords.App.Models;

namespace StudentRecords.App.Repositories
{
    public interface IStudentRepository
    {
        IEnumerable<Student> GetAll();
        Student GetById(int id);
        void Add(Student student);
        void Update(Student student);
        void Delete(int id);
    }
}
