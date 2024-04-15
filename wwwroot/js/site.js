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

// Coloca esto en tu archivo de script o dentro de una etiqueta <script> en tu vista.
$(document).ready(function () {
	$('#GuardarInquilino').submit(function (event) {
		event.preventDefault(); // Previene la recarga de la página por el envío del formulario
		var formData = $(this).serialize(); // Serializa los datos del formulario

		$.ajax({
			url: '/Inquilino/GuardarInquilino', // La URL de tu método en el controlador
			type: 'POST',
			data: formData,
			success: function (response) {
				if (response.success) {
					// Muestra el SweetAlert con la respuesta
					Swal.fire({
						icon: 'success',
						title: '¡Guardado!',
						text: response.message,
					}).then((result) => {
						if (result.isConfirmed) {
							window.location.href = '/Inquilino/Crear'; // Redirecciona al Index si el usuario da clic en 'OK'
						}
					});
				} else {
					// Manejar la situación de error
					Swal.fire({
						icon: 'error',
						title: 'Oops...',
						text: response.message,
					});
				}
			},
			error: function (xhr, status, error) {
				// Manejar error AJAX
				Swal.fire({
					icon: 'error',
					title: 'Oops...',
					text: 'Un error ocurrió: ' + error,
				});
			},
		});
	});
});

function buscarInquilinoEnVivo(inputElement) {
	var searchTerm = inputElement.value;

	// Limpiar los resultados si la búsqueda es demasiado corta
	if (searchTerm.length < 3) {
		document.getElementById('resultado-busqueda-inquilinos').innerHTML = '';
		return;
	}

	// Realiza una solicitud GET al servidor con el término de búsqueda
	fetch(`/Inquilino/BuscarEnVivo?term=${searchTerm}`)
		.then((response) => response.json())
		.then((data) => {
			var resultadosElement = document.getElementById(
				'resultado-busqueda-inquilinos'
			);
			resultadosElement.innerHTML = ''; // Limpiar resultados antiguos

			if (data.message) {
				resultadosElement.innerHTML =
					'<a href="/Inquilino/Crear">Crear nuevo inquilino</a>';
			} else {
				data.forEach((inquilino) => {
					// Construye el HTML para cada inquilino encontrado
					var inquilinoDiv = document.createElement('div');
					inquilinoDiv.innerHTML = `Nombre: ${inquilino.nombre} - Apellido: ${inquilino.apellido} - DNI: ${inquilino.dni}`;
					resultadosElement.appendChild(inquilinoDiv);
				});
			}
		})
		.catch((error) => console.error('Error al buscar inquilinos:', error));
}
