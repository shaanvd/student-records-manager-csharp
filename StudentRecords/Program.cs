using System;
using System.IO;
using StudentRecords.App.Repositories;
using StudentRecords.App.Services;

namespace StudentRecords.App;

class Program
{
    static void Main(string[] args)
    {
        string dbPath = Path.Combine("Data", "students.json");
        IStudentRepository repository = new JsonStudentRepository(dbPath);
        var service = new StudentService(repository);

        bool running = true;
        while (running)
        {
            Console.WriteLine("\n--- Student Records Engine ---");
            Console.WriteLine("1. View All Students (Diagnostic)");
            Console.WriteLine("2. Exit Console");
            Console.WriteLine("Note: Please run the StudentRecords.Api project to use the full Web Dashboard.");
            Console.Write("\nSelect an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var students = service.GetAll();
                    Console.WriteLine("\n--- Enrolled Students ---");
                    foreach (var s in students)
                    {
                        Console.WriteLine($"ID: {s.Id} | Name: {s.Name} | Email: {s.Email} | Course: {s.Course}");
                    }
                    break;
                case "2":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}