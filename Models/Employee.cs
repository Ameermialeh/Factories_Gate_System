using FactoriesGateSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class Employee  
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required, StringLength(100)]
        public string EmployeeName { get; set; }
        [Required, EmailAddress, StringLength(150)]
        public string EmployeeEmail { get; set; }
        [StringLength(200)]
        public string EmployeePassword { get; set; }

        [Required]
        public int EmployeeSalary { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? CreateAt { get; set; }

        public UserType access() => UserType.Employee;
       
        public ICollection<Vacation> Vacations { get; set; }
    }
}
