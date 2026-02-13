using Microsoft.AspNetCore.Mvc;
using MvcCoreProcedures.Models;
using MvcCoreProcedures.Repositories;

namespace MvcCoreProcedures.Controllers
{
    public class TrabajadoresController : Controller
    {
        private RepositoryEmpleados repo;
        public TrabajadoresController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            TrabajadoresModel model = await this.repo.GetTrabajadoresModelAsync();
            List<string> oficios = await this.repo.GetOficiosAsync();
            ViewBag.Oficios = oficios;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string oficio)
        {
            TrabajadoresModel model = await this.repo.GetTrabajadoresModelOficioAsync(oficio);
            List<string> oficios = await this.repo.GetOficiosAsync();
            ViewBag.Oficios = oficios;
            return View(model);
        }
    }
}
