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
                return new JsonResult(_employeeService.isEligible(employee));
            }
            catch(Exception e)
            {
                return BadRequest("failed to check employee");
            }
        }
    }
}
