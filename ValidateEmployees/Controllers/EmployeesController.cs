using Microsoft.AspNetCore.Mvc;
using System;
using ValidateEmployees.DTOs;
using ValidateEmployees.Services;

namespace ValidateEmployees.Controllers
{
    [ApiController]
    [Route("check")]
    public class EmployeesController : ControllerBase
    {
        EmployeesService _employeeService;

        public EmployeesController(EmployeesService employeesService)
        {
            _employeeService = employeesService;
        }

        [HttpPost]
        public IActionResult Check([FromBody] EmployeeDTO employee)
        {
            try
            {
                return new JsonResult(_employeeService.isEligible(employee.first_name, employee.last_name, employee.date_of_birth, employee.employee_id));
            }
            catch(Exception e)
            {
                return BadRequest("failed to check employee");
            }
        }

        [HttpGet]
        public IActionResult Check(string first_name, string last_name, string date_of_birth, int employee_id)
        {
            try 
            {
                return new JsonResult(_employeeService.isEligible(first_name, last_name, date_of_birth, employee_id));
            }
            catch (Exception e)
            {
                return BadRequest("failed to check employee");
            }

        }

    }
}
