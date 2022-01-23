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

        public EmployeeEligability isEligible(EmployeeDTO employee)
        {
            DateTime birthDate = DateTime.Parse(employee.date_of_birth);

            var employees = _helloHeartContext.Employees;

            var result = from e in employees
                         where e.EmployeeId == employee.employee_id &&
                         e.FirstName == employee.first_name &&
                         e.LastName == employee.last_name &&
                         e.DateOfBirth == birthDate
                         select e;

            var found = result.FirstOrDefault();
            if(found != null)
            {
                return new EmployeeEligability() { Eligable = found.IsActive };
            }

            return new EmployeeEligability() { Eligable = false };
        }
    }
}
