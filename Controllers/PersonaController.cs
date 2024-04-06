using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using asp.net.Models;

namespace asp.net.Controllers;

public class PersonaController : Controller
{
    private readonly ILogger<PersonaController> _logger;

    public PersonaController(ILogger<PersonaController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ControllerPersona rp = new ControllerPersona();
        var lista = rp.GetPersonas();
        return View(lista);
    }

    
}