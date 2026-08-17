using System;
using System.Collections.Generic;
using System.Text;

namespace StudentRecords.App.Models;

public abstract class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}