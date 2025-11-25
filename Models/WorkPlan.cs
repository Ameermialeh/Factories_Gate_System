using System.ComponentModel.DataAnnotations.Schema;

namespace FactoriesGateSystem.Models
{
    public class WorkPlan
    {
        public int PlaneId { get; set; }

        public string PlanName { get; set; }

        public DateTime PlanDate { get; set; }

        public string PlanDescription { get; set; }

        public int ManagerId { get; set; }
        [ForeignKey(nameof(ManagerId))]
        public Manager Manager { get; set; }
    }
}
