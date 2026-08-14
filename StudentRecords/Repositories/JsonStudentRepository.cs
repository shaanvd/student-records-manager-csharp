using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using StudentRecords.App.Models;

namespace StudentRecords.App.Repositories
{
    public class JsonStudentRepository : IStudentRepository
    {
        private readonly string _filePath;

        public JsonStudentRepository(string filePath)
        {
            _filepath = filePath;
        }

        public List<Student> getAll()
        {
            if (!File.Exists(_filePath))
                return new List<Student>();

            string json = File.ReadAllText(_filepath);
            return JsonSerializer.Deserialize<List<Student>>(json)
                ?? new List<Student>();
        }

        public void SaveAll(List<Student> students)
        {
            string? directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrWhiteSpace(directory))
                Directory.CreateDirectory(directory);

            string json = JsonSerializer.Serialize(students,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(_filePath, json);
        }
    }
}
