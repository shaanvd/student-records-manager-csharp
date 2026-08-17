using NUnit.Framework;
using StudentRecords.App.Exceptions;
using StudentRecords.App.Services;
using StudentRecords.App.Repositories;

namespace StudentRecords.Tests;

[TestFixture]
public class StudentServiceTests
{
    private StudentService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _service = new StudentService(new InMemoryStudentRepository(), new DummyLogger());
    }

    [Test]
    public void AddStudent_ValidData_AddsStudent()
    {
        _service.AddStudent(1, "Maya", 22, "Software Testing");
        Assert.That(_service.GetAll().Count, Is.EqualTo(1));
    }

    [Test]
    public void AddStudent_DuplicateId_ThrowsInvalidOperationException()
    {
        _service.AddStudent(1, "Maya", 22, "Testing");
        Assert.Throws<InvalidOperationException>(() =>
        _service.AddStudent(1, "Noah", 24, "Cloud"));
    }

    [TestCase(0)]
    [TestCase(-1)]
    public void AddStudent_InvalidId_ThrowsArgumentOutOfRangeException(int id)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        _service.AddStudent(id, "Maya", 22, "Testing"));
    }

    [Test]
    public void GetById_MissingStudent_ThrowsStudentNotFoundException()
    {
        Assert.Throws<StudentNotFoundException>(() => _service.GetById(99));
    }

    [Test]
    public void UpdateStudent_ExistingStudent_ChangesValues()
    {
        _service.AddStudent(1, "Maya", 22, "Testing");
        _service.UpdateStudent(1, "Maya Jones", 23, "DevOps");

        var student = _service.GetById(1);
        Assert.That(student.Name, Is.EqualTo("Maya Jones"));
        Assert.That(student.Age, Is.EqualTo(23));
        Assert.That(student.Course, Is.EqualTo("DevOps"));
    }

    [Test]
    public void DeleteStudent_ExistingStudent_RemovesStudent()
    {
        _service.AddStudent(1, "Maya", 22, "Testing");
        _service.DeleteStudent(1);
        Assert.That(_service.GetAll(), Is.Empty);
    }

    [TestCase("")]
    [TestCase("   ")]
    public void AddStudent_InvalidName_ThrowsArgumentException(string invalidName)
    {
        Assert.Throws<ArgumentException>(() =>
            _service.AddStudent(99, invalidName, 20, "Computer Science"));
    }

    [TestCase("")]
    [TestCase("   ")]
    public void AddStudent_InvalidCourse_ThrowsArgumentException(string invalidCourse)
    {
        Assert.Throws<ArgumentException>(() =>
            _service.AddStudent(100, "Valid Name", 20, invalidCourse));
    }
}
