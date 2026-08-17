using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;
using StudentRecords.App.Exceptions;
using StudentRecords.App.Models;
using StudentRecords.App.Repositories;
using StudentRecords.App.Logging;

namespace StudentRecords.App.Services
{
    public class StudentService
    {
        private readonly IStudentRepository _repository;
        private readonly List<Student> _students;
        private readonly ILogger _logger;

        public StudentService(IStudentRepository repository, ILogger logger)
        {
            _students = repository.GetAll();
            _repository = repository;
            _logger = logger;
        }

        public IReadOnlyList<Student> GetAll() => _students.AsReadOnly();

        public IReadOnlyList<Student> SearchByName(string partialName)
        {
            if (string.IsNullOrWhiteSpace(partialName))
                return GetAll();

            var results = _students.Where(s => s.Name.Contains(partialName, StringComparison.OrdinalIgnoreCase));

            return results.ToList().AsReadOnly();
        }

        public IReadOnlyList<Student> GetStudentsSorted(string sortBy)
        {
            var sortedResults = sortBy.ToLower() switch
            {
                "name" => _students.OrderBy(s => s.Name),
                "age" => _students.OrderBy(s => s.Age),
                "course" => _students.OrderBy(s => s.Course),
                _ => _students.OrderBy(s => s.Id) 
            };

            return sortedResults.ToList().AsReadOnly();
        }

        public Student GetById(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                _logger.LogError($"Lookup failed. Student ID {id} was not found in the database.");
                throw new StudentNotFoundException(id);
            }

            return student;
        }

        public void AddStudent(int id, string name, int age, string course)
        {
            var student = new Student
            {
                Id = id,
                Name = name.Trim(),
                Age = age,
                Course = course.Trim()
            };

            student.Validate();

            if (_students.Any(s => s.Id == id))
            {
                _logger.LogError($"Failed to add student. ID {id} already exists in the system.");
                throw new InvalidOperationException($"Student ID {id} already exists.");
            }

            _students.Add(student);
            Save();

            _logger.LogInfo($"Successfully added student: {name} (ID: {id}) to the course {course}.");
        }

        public void UpdateStudent(int id, string name, int age, string course)
        {
            Student student = GetById(id);

            student.Name = name.Trim();
            student.Age = age;
            student.Course = course.Trim();

            student.Validate();

            Save();
            _logger.LogInfo($"Successfully updated record for student ID: {id}. New name: {student.Name}.");
        }
        

        public void DeleteStudent(int id)
        {
            Student student = GetById(id);
            _students.Remove(student);
            Save();

            _logger.LogInfo($"Permanently deleted student ID: {id} from the database.");
        }

        private void Save() => _repository.SaveAll(_students);

    }
}
