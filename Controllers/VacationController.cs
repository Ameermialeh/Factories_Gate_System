using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    public class VacationController : Controller
    {
        private readonly VacationController _vacationRepo;

        public VacationController(VacationController vacationRepo)
        {
            _vacationRepo = vacationRepo;
        }


    }
}
