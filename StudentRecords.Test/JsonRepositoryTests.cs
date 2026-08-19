using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
using System.Linq; // Required for the .ToList() method
using StudentRecords.App.Models;
using StudentRecords.App.Repositories;

namespace StudentRecords.Tests;

[TestFixture]
public class JsonRepositoryTests
{
    private readonly string _testFilePath = "test_students.json";
    private JsonStudentRepository _repository = null!;

    [SetUp]
    public void SetUp()
    {
        // It is safer to delete the file BEFORE creating the repository, 
        // in case the repository constructor automatically generates a blank file.
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
        _repository = new JsonStudentRepository(_testFilePath);
    }

    [Test]
    public void Add_And_GetAll_RoundTrip_Succeeds()
    {
        // 1. Arrange: Create your test instances
        var originalStudents = new List<Student>
        {
            new Student { Id = 1, Name = "Shaan", Email = "shaan@test.com", Age = 22, Course = "Computer Science" },
            new Student { Id = 2, Name = "Maya", Email = "maya@test.com", Age = 21, Course = "Mathematics" }
        };

        // 2. Act: Loop through the list and add the actual object variables, not the Type
        foreach (var student in originalStudents)
        {
            _repository.Add(student);
        }

        // Convert the returned IEnumerable into a List so we can use index brackets
        var retrievedStudents = _repository.GetAll().ToList();

        // 3. Assert: Verify the data saved and loaded correctly
        Assert.That(retrievedStudents.Count, Is.EqualTo(2));
        Assert.That(retrievedStudents[0].Name, Is.EqualTo("Shaan"));
        Assert.That(retrievedStudents[1].Course, Is.EqualTo("Mathematics"));
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }
}