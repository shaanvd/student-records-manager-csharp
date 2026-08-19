using StudentRecords.App.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StudentRecords.App.Repositories;

public class CsvStudentRepository : IStudentRepository
{
    private readonly string _filePath;

    public CsvStudentRepository(string filePath)
    {
        _filePath = filePath;
        EnsureFileExists();
    }

    private void EnsureFileExists()
    {
        if (!File.Exists(_filePath))
        {
            var directory = Path.GetDirectoryName(_filePath);

            // NEW: Only create the directory if the path actually exists
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Keep your existing File.WriteAllText line here!
        }
    }

    public IEnumerable<Student> GetAll()
    {
        var students = new List<Student>();
        var lines = File.ReadAllLines(_filePath).Skip(1); // Skip header

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length == 5)
            {
                students.Add(new Student
                {
                    Id = int.Parse(parts[0]),
                    Name = parts[1],
                    Email = parts[2],
                    Age = int.Parse(parts[3]),
                    Course = parts[4]
                });
            }
        }
        return students;
    }

    public Student GetById(int id)
    {
        return GetAll().FirstOrDefault(s => s.Id == id)!;
    }

    public void Add(Student student)
    {
        var line = $"{student.Id},{student.Name},{student.Email},{student.Age},{student.Course}\n";
        File.AppendAllText(_filePath, line);
    }

    public void Update(Student student)
    {
        var students = GetAll().ToList();
        var index = students.FindIndex(s => s.Id == student.Id);

        if (index != -1)
        {
            students[index] = student;
            SaveAll(students);
        }
    }

    public void Delete(int id)
    {
        var students = GetAll().Where(s => s.Id != id).ToList();
        SaveAll(students);
    }

    private void SaveAll(IEnumerable<Student> students)
    {
        var lines = new List<string> { "Id,Name,Email,Age,Course" };
        lines.AddRange(students.Select(s => $"{s.Id},{s.Name},{s.Email},{s.Age},{s.Course}"));
        File.WriteAllLines(_filePath, lines);
    }
}