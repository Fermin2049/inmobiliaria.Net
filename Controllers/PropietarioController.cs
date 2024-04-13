using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using asp.net.Models;

namespace asp.net.Controllers;

public class PropietarioController : Controller
{
    private readonly ILogger<PropietarioController> _logger;

    public PropietarioController(ILogger<PropietarioController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ControllerPropietario rp = new ControllerPropietario();
        var lista = rp.GetPropietarios();
        return View(lista);
    }
    public IActionResult Buscar()
		{
			try
			{
				return View();
			}
			catch (Exception ex)
			{//poner breakpoints para detectar errores
				throw;
			}
		}

		// GET: Propietario/Buscar/5
		[Route("[controller]/Buscar/{q}", Name = "Buscar")]
		public IActionResult Buscar(string q)
		{
			try
			{
				var res = new ControllerPropietario().BuscarPorNombre(q);
				return Json(new { Datos = res });
			}
			catch (Exception ex)
			{
				return Json(new { Error = ex.Message });
			}
		}

    public IActionResult Crear()
    {
        return View();
    }

    public IActionResult Guardar(Propietario propietario)
    {
        ControllerPropietario rp = new ControllerPropietario();
        rp.CrearPropietario(propietario);
        return RedirectToAction(nameof(Index));
    }

}