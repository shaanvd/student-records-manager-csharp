using NUnit.Framework;
using System;
using System.Linq;
using StudentRecords.App.Exceptions;
using StudentRecords.App.Services;

namespace StudentRecords.Tests;

[TestFixture]
public class StudentServiceTests
{
    private StudentService _service = null!;

    [SetUp]
    public void SetUp()
    {
        // Re-initialize with a clean in-memory database before every single test
        _service = new StudentService(new InMemoryStudentRepository());
    }

    // --- FIXED EXISTING TESTS ---

    [Test]
    public void AddStudent_ValidData_AddsStudent()
    {
        _service.AddStudent(1, "Maya", 22, "Software Testing");
        Assert.That(_service.GetStudents().Count(), Is.EqualTo(1));
    }

    [Test]
    public void AddStudent_DuplicateId_ThrowsInvalidOperationException()
    {
        _service.AddStudent(1, "Maya", 22, "Testing");
        Assert.Throws<InvalidOperationException>(() =>
            _service.AddStudent(1, "Noah", 24, "Cloud"));
    }

    [Test]
    public void GetById_MissingStudent_ThrowsStudentNotFoundException()
    {
        Assert.Throws<StudentNotFoundException>(() => _service.GetById(99));
    }

    [Test]
    public void DeleteStudent_ExistingStudent_RemovesStudent()
    {
        _service.AddStudent(1, "Maya", 22, "Testing");
        _service.DeleteStudent(1);
        Assert.That(_service.GetStudents().Any(), Is.False);
    }

    // --- NEW EXTENSION CHALLENGE TESTS ---

    [Test]
    public void GetStudents_SearchPartialName_ReturnsMatchesIgnoreCase()
    {
        _service.AddStudent(1, "Shaan", 22, "Computer Science");
        _service.AddStudent(2, "Maya", 21, "Mathematics");

        // The LINQ query should find "Shaan" even with mixed casing
        var results = _service.GetStudents(search: "shA").ToList();

        Assert.That(results.Count, Is.EqualTo(1));
        Assert.That(results[0].Name, Is.EqualTo("Shaan"));
    }

    [Test]
    public void GetStudents_SortByName_ReturnsAlphabeticalOrder()
    {
        _service.AddStudent(1, "Zack", 22, "Art");
        _service.AddStudent(2, "Alice", 21, "Biology");

        var results = _service.GetStudents(sortBy: "name").ToList();

        // Alice should move to index 0, Zack to index 1
        Assert.That(results[0].Name, Is.EqualTo("Alice"));
        Assert.That(results[1].Name, Is.EqualTo("Zack"));
    }

    [Test]
    public void GetCourseSummary_ReturnsCorrectCounts()
    {
        _service.AddStudent(1, "Shaan", 22, "Computer Science");
        _service.AddStudent(2, "Maya", 21, "Computer Science");
        _service.AddStudent(3, "Zack", 20, "Art");

        var summary = _service.GetCourseSummary();

        Assert.That(summary["Computer Science"], Is.EqualTo(2));
        Assert.That(summary["Art"], Is.EqualTo(1));
    }

    [TestCase("")]
    [TestCase("   ")]
    public void AddStudent_InvalidName_ThrowsArgumentException(string invalidName)
    {
        Assert.Throws<ArgumentException>(() =>
            _service.AddStudent(1, invalidName, 22, "Testing"));
    }

    [TestCase("")]
    [TestCase("   ")]
    public void AddStudent_InvalidCourse_ThrowsArgumentException(string invalidCourse)
    {
        Assert.Throws<ArgumentException>(() =>
            _service.AddStudent(1, "Valid Name", 22, invalidCourse));
    }
}