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
