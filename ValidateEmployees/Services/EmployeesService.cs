using DBModel.Models;
using System;
using System.Linq;
using ValidateEmployees.DTOs;

namespace ValidateEmployees.Services
{
    public class EmployeesService
    {
        HelloHeartContext _helloHeartContext;

        public EmployeesService(HelloHeartContext helloHeartContext)
        {
            _helloHeartContext = helloHeartContext;
        }

        public EmployeeEligability isEligible(string first_name, string last_name, string date_of_birth, int employee_id)
        {
            DateTime birthDate = DateTime.Parse(date_of_birth);

            var employees = _helloHeartContext.Employees;

            var result = from e in employees
                         where e.EmployeeId == employee_id &&
                         e.FirstName == first_name &&
                         e.LastName == last_name &&
                         e.DateOfBirth == birthDate
                         select e;

            var found = result.FirstOrDefault();
            if (found != null)
            {
                return new EmployeeEligability() { Eligable = found.IsActive };
            }

            return new EmployeeEligability() { Eligable = false };
        }
    }
}
