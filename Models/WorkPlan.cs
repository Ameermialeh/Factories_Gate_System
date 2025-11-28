using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoriesGateSystem.Models
{
    public class WorkPlan
    {
        [Key]
        public int PlaneId { get; set; }
        [Required,StringLength(100)]
        public string PlanName { get; set; }

        public DateTime PlanDate { get; set; }
        [StringLength(300)]
        public string PlanDescription { get; set; }

        public int ManagerId { get; set; }
        [ForeignKey(nameof(ManagerId))]
        public Manager Manager { get; set; }
    }
}
