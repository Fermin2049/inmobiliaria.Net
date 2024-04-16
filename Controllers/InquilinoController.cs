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
        var inquilinos = await _repositorioInquilino.BuscarEnVivo(term) ?? new List<Inquilino>(); // Asegúrate de devolver una lista vacía si es null
        return Json(new { inquilinos }); // Siempre devuelve una estructura JSON con la lista de inquilinos
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

    [HttpGet]
    public IActionResult ObtenerInquilino(int inquilinoID)
    {
        var inquilino = _repositorioInquilino.ObtenerInquilinoPorId(inquilinoID);
        if (inquilino != null)
        {
            return Json(new { success = true, data = inquilino });
        }
        else
        {
            return Json(new { success = false, message = "Inquilino no encontrado." });
        }
    }

    // En InquilinoController.cs
    [HttpPost]
    public async Task<IActionResult> GuardarEdicionInquilino(
        int inquilinoID,
        [FromBody] Inquilino datosEditados
    )
    {
        // Asegúrate de establecer el InquilinoID en los datos editados antes de actualizar
        datosEditados.InquilinoID = inquilinoID;
        var resultado = await _repositorioInquilino.ActualizarInquilino(datosEditados);

        if (resultado)
        {
            return Json(new { success = true, message = "Inquilino actualizado con éxito." });
        }
        else
        {
            return Json(new { success = false, message = "Error al actualizar el inquilino." });
        }
    }
}
