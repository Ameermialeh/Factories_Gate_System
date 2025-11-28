using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoriesGateSystem.Models
{
    public class EmployeeMaterial
    {
        [Key]
        public int EmployeeMaterialId { get; set; } 


        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }


        public int MaterialId { get; set; }
        [ForeignKey(nameof(MaterialId))]
        public Material Material { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public DateTime Date { get; set; }
    }
}
