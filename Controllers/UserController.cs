using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _appDbContext; 

        public UserController (AppDbContext Db)
        {
            this._appDbContext = Db;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
