using System;
using System.Collections.Generic;
using System.Text;

namespace StudentRecords.App.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Course { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Id,-5} {Name,-20} {Age,-5} {Course}";
        }
    }
}
