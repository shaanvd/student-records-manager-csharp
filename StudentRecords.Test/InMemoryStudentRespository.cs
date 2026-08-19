using System.Collections.Generic;
using System.Linq;
using StudentRecords.App.Models;
using StudentRecords.App.Repositories;

namespace StudentRecords.Tests;

public class InMemoryStudentRepository : IStudentRepository
{
    private List<Student> _students = new();

    public IEnumerable<Student> GetAll() => _students;

    public Student GetById(int id) => _students.FirstOrDefault(s => s.Id == id)!;

    public void Add(Student student) => _students.Add(student);

    public void Update(Student student)
    {
        var index = _students.FindIndex(s => s.Id == student.Id);
        if (index != -1) _students[index] = student;
    }

    public void Delete(int id)
    {
        var student = GetById(id);
        if (student != null) _students.Remove(student);
    }
}