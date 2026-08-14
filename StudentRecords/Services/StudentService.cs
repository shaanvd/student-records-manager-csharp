using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;
using StudentRecords.App.Exceptions;
using StudentRecords.App.Models;
using StudentRecords.App.Repositories;

namespace StudentRecords.App.Services
{
    public class StudentService
    {
        private readonly IStudentRepository _repository;
        private readonly List<Student> _students;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
            _students = repository.GetAll();
        }

        public IReadOnlyList<Student> GetAll() => _students.AsReadOnly();

        public Student GetById(int id)
        {
            return _students.FirstOrDefault(s => s.Id == id)
                ?? throw new StudentNotFoundException(id);
        }

        public void AddStudent(int id, string name, int age, string course)
        {
            Validate(id, name, age, course);

            if (_students.Any(s => s.Id == id))
                throw new InvalidOperationException($"Student ID {id} already exists.");

            _students.Add(new Student
            {
                Id = id,
                Name = name.Trim(),
                Age = age,
                Course = course.Trim()
            });

            Save();
        }

        public void DeleteStudent(int id)
        {
            Student student = GetById(id);
            _students.Remove(student);
            Save();
        }

        private void Save() => _repository.SaveAll(_students);

        private static void Validate(int id, string name, int age, string course)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "ID must be positive.");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));
            if (age < 16 || age > 120)
                throw new ArgumentOutOfRangeException(nameof(age), "Age must be between 16 and 120.");
            if (string.IsNullOrWhiteSpace(course))
                throw new ArgumentException("Course is required.", nameof(course));
        }

    }
}
