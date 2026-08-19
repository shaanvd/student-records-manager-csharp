using StudentRecords.App.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace StudentRecords.App.Repositories;

public class JsonStudentRepository : IStudentRepository
{
    private readonly string _filePath;

    public JsonStudentRepository(string filePath)
    {
        _filePath = filePath;
        EnsureFileExists();
    }

    private void EnsureFileExists()
    {
        if (!File.Exists(_filePath))
        {
            var directory = Path.GetDirectoryName(_filePath);


            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllText(_filePath, "[]");
        }
    }

    public IEnumerable<Student> GetAll()
    {
        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();
    }

    public Student GetById(int id)
    {
        return GetAll().FirstOrDefault(s => s.Id == id)!;
    }

    public void Add(Student student)
    {
        var students = GetAll().ToList();
        students.Add(student);
        SaveAll(students);
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
        var json = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}