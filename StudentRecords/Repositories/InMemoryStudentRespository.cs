using StudentRecords.App.Models;
using StudentRecords.App.Repositories;


namespace StudentRecords.App.Repositories;

public class InMemoryStudentRepository : IStudentRepository
{
    private List<Student> _students = new();

    public List<Student> GetAll()
    {
        return _students.Select(s => new Student
        {
            Id = s.Id,
            Name = s.Name,
            Age = s.Age,
            Course = s.Course
        }).ToList();
    }

    public void SaveAll(List<Student> students)
    {
        _students = students.Select(s => new Student
        {
            Id = s.Id,
            Name = s.Name,
            Age = s.Age,
            Course = s.Course
        }).ToList();
    }
}