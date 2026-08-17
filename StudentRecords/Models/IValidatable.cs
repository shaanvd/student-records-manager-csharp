using System;
using System.Collections.Generic;
using System.Text;

namespace StudentRecords.App.Models;

public interface IValidatable
{
    void Validate();
}