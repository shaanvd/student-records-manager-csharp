using StudentRecords.App.Exceptions;
using StudentRecords.App.Services;
using StudentRecords.App.Repositories;
using System.Xml;
using StudentRecords.App.Logging;

IStudentRepository repository = new CsvStudentRepository(Path.Combine("Data", "students.csv"));

ILogger logger = new FileLogger(Path.Combine("Data", "system_logs.txt"));

StudentService service = new StudentService(repository, logger);

while (true)
{

    Console.Clear();
    Console.WriteLine("====================================");
    Console.WriteLine(" STUDENT RECORDS MANAGER");
    Console.WriteLine("====================================");
    Console.WriteLine("1. Add student");
    Console.WriteLine("2. List students");
    Console.WriteLine("3. Find student by ID");
    Console.WriteLine("4. Update student");
    Console.WriteLine("5. Delete student");
    Console.WriteLine("6. Search by partial name"); 
    Console.WriteLine("7. List students sorted");   
    Console.WriteLine("8. Exit");                   
    Console.Write("Choice: ");

    string choice = Console.ReadLine() ?? string.Empty;

    try
    {
        switch (choice)
        {
            case "1": AddStudent(service); break;
            case "2": ListStudents(service); break;
            case "3": FindStudent(service); break;
            case "4": UpdateStudent(service); break;
            case "5": DeleteStudent(service); break;
            case "6": SearchPartialName(service); break; // NEW
            case "7": ListStudentsSorted(service); break; // NEW
            case "8": return; // UPDATED
            default: Console.WriteLine("Invalid menu option."); break;
        }
    } 
    catch (StudentNotFoundException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Validation error: {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (IOException ex)
    {
        Console.WriteLine($"File error: {ex.Message}");
    }

    Console.WriteLine("Press Enter to continue...");
    Console.ReadLine();
}

static int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out int value))
            return value;
        Console.WriteLine("Please enter a valid number");
    }

}

static string ReadRequired(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string value = (Console.ReadLine() ?? string.Empty).Trim();
        if (value.Length > 0) return value;
        Console.WriteLine("A value is required.");
    }
}

static void AddStudent(StudentService service)
{
    int id = ReadInt("ID: ");
    string name = ReadRequired("Name: ");
    int age = ReadInt("Age: ");
    string course = ReadRequired("Course: ");
    service.AddStudent(id, name, age, course);
    Console.WriteLine("Student added.");
}

static void ListStudents(StudentService service)
{
    Console.WriteLine("ID Name Age Course");
    Console.WriteLine(new string('-', 55));
    foreach (var student in service.GetAll())
        Console.WriteLine(student);
}

static void FindStudent(StudentService service)
{
    int id = ReadInt("Student ID: ");
    Console.WriteLine(service.GetById(id));
}

static void UpdateStudent(StudentService service)
{
    int id = ReadInt("Student ID: ");
    string name = ReadRequired("New name: ");
    int age = ReadInt("New age: ");
    string course = ReadRequired("New course: ");
    service.UpdateStudent(id, name, age, course);
    Console.WriteLine("Student updated.");
}

static void DeleteStudent(StudentService service)
{
    int id = ReadInt("Student ID: ");
    service.DeleteStudent(id);
    Console.WriteLine("Student deleted.");
}

static void SearchPartialName(StudentService service)
{
    string query = ReadRequired("Enter name (or part of name) to search: ");
    var results = service.SearchByName(query);

    Console.WriteLine("\nID    Name                 Age   Course");
    Console.WriteLine(new string('-', 55));

    if (results.Count == 0) Console.WriteLine("No students found.");
    foreach (var student in results) Console.WriteLine(student);
}

static void ListStudentsSorted(StudentService service)
{
    string sortBy = ReadRequired("Sort by (type 'name', 'age', or 'course'): ");
    var results = service.GetStudentsSorted(sortBy);

    Console.WriteLine("\nID    Name                 Age   Course");
    Console.WriteLine(new string('-', 55));
    foreach (var student in results) Console.WriteLine(student);
}