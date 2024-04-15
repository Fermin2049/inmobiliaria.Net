using System.Threading.Tasks;
using asp.net.Models; // Asegúrate de que tus modelos estén en este namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Si usas ILogger, necesitas este namespace

namespace asp.net.Controllers;

public class InquilinoController : Controller
{
    private readonly ILogger<InquilinoController> _logger;
    private readonly RepositorioInquilino _repositorioInquilino; // Asegúrate de tener un repositorio para inquilinos

    // Constructor con dependencias inyectadas
    public InquilinoController(
        ILogger<InquilinoController> logger,
        RepositorioInquilino repositorioInquilino
    )
    {
        _logger = logger;
        _repositorioInquilino = repositorioInquilino;
    }

    public IActionResult Index()
    {
        var lista = _repositorioInquilino.GetInquilinos();
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
            _logger.LogError(ex, "Error al buscar inquilinos");
            return View("Error");
        }
    }

    [HttpGet]
    public async Task<IActionResult> BuscarEnVivo(string term)
    {
        var inquilinos = await _repositorioInquilino.BuscarEnVivo(term);
        if (inquilinos.Count > 0)
        {
            return Json(inquilinos);
        }
        else
        {
            return Json(new { message = "No se encontraron resultados. ¿Desea crear uno nuevo?" });
        }
    }

    public IActionResult Crear()
    {
        return View();
    }

    public IActionResult GuardarInquilino(Inquilino inquilino)
    {
        var resultado = _repositorioInquilino.CrearInquilino(inquilino);
        if (resultado > 0) // Asumiendo que CrearInquilino retorna el ID del inquilino creado
        {
            return Json(new { success = true, message = "Inquilino guardado correctamente." });
        }
        else
        {
            return Json(
                new { success = false, message = "Ocurrió un error al guardar el inquilino." }
            );
        }
    }
}
