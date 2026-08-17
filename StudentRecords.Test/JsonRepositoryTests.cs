using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
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
        _repository = new JsonStudentRepository(_testFilePath);
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }

    [Test]
    public void SaveAll_And_GetAll_RoundTrip_Succeeds()
    {
        var originalStudents = new List<Student>
        {
            new Student { Id = 1, Name = "Shaan", Age = 22, Course = "Computer Science" },
            new Student { Id = 2, Name = "Maya", Age = 21, Course = "Mathematics" }
        };

        _repository.SaveAll(originalStudents);
        var retrievedStudents = _repository.GetAll();

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