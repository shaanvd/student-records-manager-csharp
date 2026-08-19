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
    public IActionResult GetAll()
    {
        var students = _service.GetAll();
        return Ok(students);
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
            _service.AddStudent(request.Id, request.Name, request.Age, request.Course);
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
            _service.UpdateStudent(id, request.Name, request.Age, request.Course);
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
}