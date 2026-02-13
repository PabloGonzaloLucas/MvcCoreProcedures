using Microsoft.AspNetCore.Mvc;
using MvcCoreProcedures.Models;
using MvcCoreProcedures.Repositories;

namespace MvcCoreProcedures.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;
        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<VistaEmpleado> empleados = await this.repo.GetVistaEmpleadosAsync();
            return View(empleados);
        }
    }
}
