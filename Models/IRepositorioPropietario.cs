using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net.Models;

	public interface IRepositorioPropietario : IRepositorio<Propietario>
	{
		IList<Propietario> BuscarPorNombre(string nombre);
	} 
