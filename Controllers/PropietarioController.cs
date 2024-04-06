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

    
}