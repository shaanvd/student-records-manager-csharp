using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StudentRecords.App.Models;

namespace StudentRecords.App.Repositories;

public class CsvStudentRepository : IStudentRepository
{
    private readonly string _filePath;

    public CsvStudentRepository(string filePath)
    {
        _filePath = filePath;
    }

    public List<Student> GetAll()
    {
        if (!File.Exists(_filePath))
            return new List<Student>();

        var students = new List<Student>();
        var lines = File.ReadAllLines(_filePath);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            var parts = lines[i].Split(',');

            if (parts.Length == 4)
            {
                students.Add(new Student
                {
                    Id = int.Parse(parts[0]),
                    Name = parts[1],
                    Age = int.Parse(parts[2]),
                    Course = parts[3]
                });
            }
        }

        return students;
    }

    public void SaveAll(List<Student> students)
    {
        string? directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(directory))
            Directory.CreateDirectory(directory);

        var lines = new List<string>();

        lines.Add("Id,Name,Age,Course");

        foreach (var student in students)
        {
            lines.Add($"{student.Id},{student.Name},{student.Age},{student.Course}");
        }

        File.WriteAllLines(_filePath, lines);
    }
}