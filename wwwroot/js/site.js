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
//----------------------PROPIETARIO-----------------------
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
//----------------------INQUILINO-----------------------

$(document).ready(function () {
	$('#GuardarInquilino').submit(function (event) {
		event.preventDefault(); // Previene la recarga de la página por el envío del formulario
		var formData = $(this).serialize(); // Serializa los datos del formulario

		$.ajax({
			url: '/Inquilino/GuardarInquilino',
			type: 'POST',
			data: formData,
			success: function (response) {
				if (response.success) {
					Swal.fire({
						icon: 'success',
						title: '¡Guardado!',
						text: response.message,
					}).then((result) => {
						if (result.isConfirmed) {
							window.location.href = '/Inquilino/Crear';
						}
					});
				} else {
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

	if (searchTerm.length < 3) {
		document.getElementById('resultado-busqueda-inquilinos').innerHTML = '';
		return;
	}

	fetch(`/Inquilino/BuscarEnVivo?term=${searchTerm}`)
		.then((response) => response.json())
		.then((data) => {
			var resultadosElement = document.getElementById(
				'resultado-busqueda-inquilinos'
			);
			resultadosElement.innerHTML = '';

			if (data.inquilinos && data.inquilinos.length > 0) {
				var tabla =
					'<table class="table table-custom"><thead><tr>' +
					'<th>Nombre</th>' +
					'<th>Apellido</th>' +
					'<th>DNI</th>' +
					'<th>Email</th>' +
					'<th>Teléfono</th>' +
					'<th>Acciones</th></tr></thead><tbody>';
				data.inquilinos.forEach((inquilino) => {
					tabla += `<tr>
								  <td>${inquilino.nombre}</td>
								  <td>${inquilino.apellido}</td>
								  <td>${inquilino.dni}</td>
								  <td>${inquilino.email}</td>
								  <td>${inquilino.telefono}</td>
								  <td><button onclick="editarInquilino(${inquilino.inquilinoID})" class="btn btn-primary"><i class="fa fa-pencil-alt"></i></button></td>
								  </tr>`;
				});

				tabla += '</tbody></table>';
				resultadosElement.innerHTML = tabla;
			} else {
				resultadosElement.innerHTML =
					'<p>No se encontraron inquilinos.</p>' +
					'<a class="btn btn-primary" href="/Inquilino/Crear">Crear Inquilino</a>';
			}
		})
		.catch((error) => {
			console.error('Error al buscar inquilinos:', error);
			resultadosElement.innerHTML =
				'<p>Error al realizar la búsqueda. Intente nuevamente más tarde.</p>';
		});
}

function editarInquilino(inquilinoID) {
	fetch(`/Inquilino/ObtenerInquilino?inquilinoID=${inquilinoID}`)
		.then((response) => response.json())
		.then((response) => {
			if (response.success) {
				const inquilino = response.data;
				Swal.fire({
					title: 'Editar Inquilino',
					html: `
                    <input id="swal-nombre" class="swal2-input" placeholder="Nombre" value="${
											inquilino.nombre || ''
										}">
                    <input id="swal-apellido" class="swal2-input" placeholder="Apellido" value="${
											inquilino.apellido || ''
										}">
					<input id="swal-dni" class="swal2-input" placeholder="DNI" value="${
						inquilino.dni || ''
					}">
					<input id="swal-email" class="swal2-input" placeholder="Email" value="${
						inquilino.email || ''
					}">
					<input id="swal-telefono" class="swal2-input" placeholder="Telefón" value="${
						inquilino.telefono || ''
					}">
                    `,
					focusConfirm: false,
					preConfirm: () => {
						return {
							nombre: document.getElementById('swal-nombre').value,
							apellido: document.getElementById('swal-apellido').value,
							dni: document.getElementById('swal-dni').value,
							email: document.getElementById('swal-email').value,
							telefono: document.getElementById('swal-telefono').value,
						};
					},
				}).then((result) => {
					if (result.isConfirmed) {
						guardarEdicionInquilino(inquilinoID, result.value);
					}
				});
			} else {
				Swal.fire('Error', 'Inquilino no encontrado.', 'error');
			}
		})
		.catch((error) => {
			console.error('Error:', error);
			Swal.fire(
				'Error',
				'Ocurrió un error al obtener los datos del inquilino.',
				'error'
			);
		});
}

function guardarEdicionInquilino(inquilinoID, datosEditados) {
	$.ajax({
		url: `/Inquilino/GuardarEdicionInquilino?inquilinoID=${inquilinoID}`,
		type: 'POST',
		contentType: 'application/json',
		data: JSON.stringify(datosEditados),
		success: function (response) {
			Swal.fire(
				'Guardado',
				'El inquilino ha sido actualizado.',
				'success'
			).then(() => {
				window.location.href = '/Inquilino/Buscar';
			});
		},
		error: function (xhr, status, error) {
			Swal.fire('Error', 'Hubo un problema al guardar la edición.', 'error');
		},
	});
}
