using Microsoft.AspNetCore.Mvc;
using StudentRecords.App.Services;
using StudentRecords.App.Exceptions;
using System;

namespace StudentRecords.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly StudentService _service;

    public StudentsController(StudentService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] string search = "", [FromQuery] string sortBy = "id")
    {
        return Ok(_service.GetStudents(search, sortBy));
    }

    [HttpGet("summary")]
    public IActionResult GetSummary()
    {
        return Ok(_service.GetCourseSummary());
    }

    [HttpGet("export")]
    public IActionResult ExportCsv()
    {
        var students = _service.GetStudents();
        var csvLines = new List<string> { "Id,Name,Email,Age,Course" };
        csvLines.AddRange(students.Select(s => $"{s.Id},{s.Name},{s.Email},{s.Age},{s.Course}"));

        var bytes = System.Text.Encoding.UTF8.GetBytes(string.Join("\n", csvLines));
        return File(bytes, "text/csv", "students_export.csv");
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        try
        {
            var student = _service.GetById(id);
            return Ok(student);
        }
        catch (StudentNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public IActionResult Add([FromBody] StudentRequest request)
    {
        try
        {
            _service.AddStudent(request.Id, request.Name, request.Age, request.Course, request.Email);
            return CreatedAtAction(nameof(GetById), new { id = request.Id }, request);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] StudentRequest request)
    {
        try
        {
            _service.UpdateStudent(id, request.Name, request.Age, request.Course, request.Email);
            return NoContent();
        }
        catch (StudentNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _service.DeleteStudent(id);
            return NoContent();
        }
        catch (StudentNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

public class StudentRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Course { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}