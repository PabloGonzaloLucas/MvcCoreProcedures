using Microsoft.AspNetCore.Mvc;
using MvcCoreProcedures.Models;
using MvcCoreProcedures.Repositories;

namespace MvcCoreProcedures.Controllers
{
    public class DoctoresController : Controller
    {
        private RepositoryDoctores repo;
        public DoctoresController(RepositoryDoctores repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Especialidades()
        {
            List<string> especialidades = await this.repo.GetEspecialidadesAsync();
            ViewBag.especialidades = especialidades;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Especialidades(string especialidad, int incremento, string accion)
        {
            if (accion == "incrementar")
            {
                await this.repo.UpdateDoctoresEspecialidad(especialidad, incremento);
            }
            else if (accion == "incrementarEf")
            {
                await this.repo.UpdateDoctoresEspecialidadEF(especialidad, incremento);
            }
            List<Doctor> doctors = await this.repo.GetDoctoresEspecialidad(especialidad);
            List<string> especialidades = await this.repo.GetEspecialidadesAsync();
            ViewBag.especialidades = especialidades;
            return View(doctors);
        }
    }
}
