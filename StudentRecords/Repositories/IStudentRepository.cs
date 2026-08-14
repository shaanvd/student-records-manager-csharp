using System;
using System.Collections.Generic;
using System.Text;
using StudentRecords.App.Models;

namespace StudentRecords.App.Repositories
{
    public interface IStudentRepository
    {
        List<Student> GetAll();
        void SaveAll(List<Student> students);
    }
}
