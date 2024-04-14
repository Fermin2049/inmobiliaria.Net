$(document).ready(function () {
	// Ocultar el menú de "Crear Propietario" al cargar la página
	$('#crud-propietario-menu').hide();

	// Mostrar u ocultar el menú de "Crear Propietario" cuando se haga clic en "Propietarios"
	$('#propietario-link').click(function () {
		$('#crud-propietario-menu').toggle();
	});
});

$(document).ready(function () {
	// Ocultar el menú de "Crear Propietario" al cargar la página
	$('#crud-inquilino-menu').hide();

	// Mostrar u ocultar el menú de "Crear Propietario" cuando se haga clic en "Propietarios"
	$('#inquilino-link').click(function () {
		$('#crud-inquilino-menu').toggle();
	});
});

function mostrarAlerta(mensaje, tipo) {
	Swal.fire({
		icon: tipo,
		title: mensaje,
		showConfirmButton: false,
		timer: 1500,
	});
}

function buscarPropietarioEnVivo(inputElement) {
	var searchTerm = inputElement.value;

	// Limpiar los resultados si la búsqueda es demasiado corta
	if (searchTerm.length < 3) {
		document.getElementById('resultado-busqueda').innerHTML = '';
		return;
	}

	// Realiza una solicitud GET al servidor con el término de búsqueda
	fetch(`/Propietario/BuscarEnVivo?term=${searchTerm}`)
		.then((response) => response.json())
		.then((data) => {
			var resultadosElement = document.getElementById('resultado-busqueda');
			resultadosElement.innerHTML = ''; // Limpiar resultados antiguos

			if (data.message) {
				resultadosElement.innerHTML =
					'<a href="/Propietario/Crear">Crear nuevo propietario</a>';
			} else {
				data.forEach((propietario) => {
					// Construye el HTML para cada propietario encontrado
					var propietarioDiv = document.createElement('div');
					propietarioDiv.innerHTML = `Nombre: ${propietario.nombre} - Apellido: ${propietario.apellido} - DNI: ${propietario.dni}`;
					resultadosElement.appendChild(propietarioDiv);
				});
			}
		})
		.catch((error) => console.error('Error al buscar propietarios:', error));
}
