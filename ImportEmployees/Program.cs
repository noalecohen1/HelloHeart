using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using System.Linq;
using System.IO;
using DBModel.Models;

namespace ImportEmployees
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 2)
            {
                Console.WriteLine("Faild because file path or client ID were not supplied");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                Environment.Exit(-1);
            }

            string filePath = args[0];

            int clientId;
            if(!int.TryParse(args[1], out clientId))
            {
                Console.WriteLine("faild because client ID is invalid");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                Environment.Exit(-1);
            }

            if(!File.Exists(filePath) || !filePath.EndsWith(".csv"))
            {
                Console.WriteLine("faild because the file path is invalid");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                Environment.Exit(-1);
            }


            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            HelloHeartContext db = new HelloHeartContext();
            var employees = db.Employees;

            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                if (!parser.EndOfData) 
                {
                    string[] headers = parser.ReadFields();
                    for (int i = 0; i < headers.Length; i++) 
                    {
                        switch (headers[i])
                        {
                            case "employee ID":
                            case "date of birth":
                            case "last name":
                            case "first name":
                                dictionary[headers[i]] = i;
                                break;
                            default:
                                break;
                        }
                    }
                }

                if(dictionary.Count < 4)
                {
                    Console.WriteLine("faild because the file is invalid");
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadLine();
                    Environment.Exit(-1);
                }

                List<Employee> employeesToUpdate = new List<Employee>();
                
                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();

                    employeesToUpdate.Add(new Employee() 
                    { 
                        ClientId = clientId,
                        EmployeeId = int.Parse(fields[dictionary["employee ID"]]),
                        FirstName = fields[dictionary["first name"]], 
                        LastName = fields[dictionary["last name"]],
                        DateOfBirth = DateTime.Parse(fields[dictionary["date of birth"]]),
                        IsActive = true
                    });
                }

                employeesToUpdate.ForEach(e => addOrUpdate(employees, e));
                setNotActive(employees, employeesToUpdate, clientId);

                db.SaveChanges();
            }

        }

        static void addOrUpdate(DbSet<Employee> employees, Employee employee)
        {
            var result = from e in employees
                         where e.ClientId == employee.ClientId && e.EmployeeId == employee.EmployeeId
                         select e;

            if(result.Any())
            {
                employees.Update(employee);
            }
            else
            {
                employees.Add(employee);
            }
        }

        static void setNotActive(DbSet<Employee> employees, List<Employee> employeesToUpdate, int clientId)
        {

            var result = from e in employees
                         where e.ClientId == clientId
                         select e;

            foreach(Employee employee in result)
            {
                if (!employeesToUpdate.Any(e => e.EmployeeId == employee.EmployeeId))
                {
                    employee.IsActive = false;
                }
            }
        }
    }
}
