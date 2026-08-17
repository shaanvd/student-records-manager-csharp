using System;

namespace StudentRecords.App.Models;

public class Student : Person, IValidatable
{
    public int Id { get; set; }
    public string Course { get; set; } = string.Empty;

    public void Validate()
    {
        if (Id <= 0)
            throw new ArgumentOutOfRangeException(nameof(Id), "ID must be positive.");
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("Name is required.", nameof(Name));
        if (Age < 16 || Age > 120)
            throw new ArgumentOutOfRangeException(nameof(Age), "Age must be between 16 and 120.");
        if (string.IsNullOrWhiteSpace(Course))
            throw new ArgumentException("Course is required.", nameof(Course));
    }

    public override string ToString()
    {
        return $"{Id,-5} {Name,-20} {Age,-5} {Course}";
    }
}