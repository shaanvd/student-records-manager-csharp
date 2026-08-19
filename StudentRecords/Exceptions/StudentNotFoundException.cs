using System;

namespace StudentRecords.App.Exceptions;

public class StudentNotFoundException : Exception
{
    public StudentNotFoundException(string message) : base(message)
    {
    }
}