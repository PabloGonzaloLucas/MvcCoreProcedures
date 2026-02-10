using Microsoft.AspNetCore.Mvc;
using MvcCoreProcedures.Models;
using MvcCoreProcedures.Repositories;

namespace MvcCoreProcedures.Controllers
{
    public class EnfermosController : Controller
    {
        private RepositoryEnfermos repo;
        public EnfermosController(RepositoryEnfermos repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Enfermo> enfermos = await this.repo.GetEnfermos();
            return View(enfermos);
        }
    }
}
