using FactoriesGateSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace FactoriesGateSystem.Models
{
    public class Employee :User
    {
        [Required]
        public int EmployeeSalary { get; set; }

        public override UserType access() => UserType.Employee;
       

        public ICollection<EmployeeMaterial> EmployeeMaterials { get; set; }
        public ICollection<Vacation> Vacations { get; set; }
    }
}
