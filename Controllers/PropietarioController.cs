using Microsoft.AspNetCore.Mvc;
using asp.net.Models; // Asegúrate de que tus modelos estén en este namespace
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Si usas ILogger, necesitas este namespace

namespace asp.net.Controllers;

public class PropietarioController : Controller
{
    private readonly ILogger<PropietarioController> _logger;
    private readonly RepositorioPropietario _repositorioPropietario;

    // Constructor único con todas las dependencias requeridas
    public PropietarioController(ILogger<PropietarioController> logger, RepositorioPropietario repositorioPropietario)
    {
        _logger = logger;
        _repositorioPropietario = repositorioPropietario;
    }

    public IActionResult Index()
    {
        // Usa la dependencia inyectada, no crees una nueva instancia
        var lista = _repositorioPropietario.GetPropietarios();
        return View(lista);
    }

    public IActionResult Buscar()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar propietarios");
            return View("Error"); // Retorna a una vista de error o maneja como creas conveniente
        }
    }

    // Puedes mantener este si tienes una ruta específica con un parámetro
    [HttpGet("[controller]/Buscar/{q}", Name = "Buscar")]
    public IActionResult Buscar(string q)
    {
        try
        {
            // Aquí también deberías usar la dependencia inyectada
            var res = _repositorioPropietario.BuscarPorNombre(q);
            return Json(new { Datos = res });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar por nombre");
            return Json(new { Error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> BuscarEnVivo(string term)
    {
        var propietarios = await _repositorioPropietario.BuscarEnVivo(term);
        if (propietarios.Count > 0)
        {
            return Json(propietarios);
        }
        else
        {
            // Devuelve un mensaje o un objeto que la función de JS pueda entender para mostrar un enlace de creación.
            return Json(new { message = "No se encontraron resultados. ¿Desea crear uno nuevo?" });
        }
    }

    public IActionResult Crear()
    {
        return View();
    }

    public IActionResult Guardar(Propietario propietario)
    {
        // De nuevo, usa la dependencia inyectada
        _repositorioPropietario.CrearPropietario(propietario);
        return RedirectToAction(nameof(Index));
    }
}
