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

        [Required]
        public int EmployeeSalary { get; set; }

        public DateTime? CreateAt { get; set; }

        public UserType access() => UserType.Employee;
       
        public ICollection<Vacation> Vacations { get; set; }
    }
}
