using StudentRecords.App.Exceptions;
using StudentRecords.App.Models;
using StudentRecords.App.Repositories;
using System;
using System.Collections.Generic;

namespace StudentRecords.App.Services;

public class StudentService
{
    private readonly IStudentRepository _repository;

    public StudentService(IStudentRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Student> GetAll()
    {
        return _repository.GetAll();
    }

    public Student GetById(int id)
    {
        var student = _repository.GetById(id);
        if (student == null) throw new StudentNotFoundException($"Student with ID {id} not found.");
        return student;
    }

    public void AddStudent(int id, string name, int age, string course, string email = "")
    {
        if (_repository.GetById(id) != null) throw new InvalidOperationException("Student ID already exists.");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.");
        if (string.IsNullOrWhiteSpace(course)) throw new ArgumentException("Course cannot be empty.");

        _repository.Add(new Student { Id = id, Name = name, Age = age, Course = course, Email = email });
    }

    public void UpdateStudent(int id, string name, int age, string course, string email = "")
    {
        var existing = GetById(id);
        existing.Name = name;
        existing.Age = age;
        existing.Course = course;
        existing.Email = email;

        _repository.Update(existing);
    }

    public void DeleteStudent(int id)
    {
        GetById(id); // Throws if not found
        _repository.Delete(id);
    }

    public IEnumerable<Student> GetStudents(string search = "", string sortBy = "id")
    {
        var query = _repository.GetAll();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(s =>
                s.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                s.Course.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                s.Id.ToString().Contains(search));
        }

        return sortBy.ToLower() switch
        {
            "name" => query.OrderBy(s => s.Name),
            "course" => query.OrderBy(s => s.Course),
            "age" => query.OrderBy(s => s.Age),
            _ => query.OrderBy(s => s.Id)
        };
    }

    public Dictionary<string, int> GetCourseSummary()
    {
        return _repository.GetAll()
            .GroupBy(s => s.Course)
            .ToDictionary(g => g.Key, g => g.Count());
    }
}