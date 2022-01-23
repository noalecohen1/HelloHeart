using System;
using System.Collections.Generic;

#nullable disable

namespace DBModel.Models
{
    public partial class Employee
    {
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
    }
}
