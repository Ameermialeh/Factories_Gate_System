using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly AdminRepo _adminRepo;

        public AdminController(AdminRepo adminRepo)
        {
            _adminRepo = adminRepo;
        }


    }
}
