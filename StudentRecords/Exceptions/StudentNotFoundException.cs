using System;
using System.Collections.Generic;
using System.Text;

namespace StudentRecords.App.Exceptions
{
    public class StudentNotFoundException : Exception
    {
        public StudentNotFoundException(int id)
        : base($"Student with ID {id} was not found.")
        {
        }

    }
}