var serviciosSeleccionados = [];

$('#selectServicios').change(function () {
    var selectedServicioId = $(this).val();

    $.ajax({
        url: '/Paquetes/ObtenerCostoServicio',
        type: 'GET',
        data: { servicioId: selectedServicioId },
        dataType: 'json',
        success: function (data) {
            $('#inputCostoServicio').val(data.costo);
        },
        error: function () {
            console.error('Error al obtener el costo del servicio.');
        }
    });
});

$('#btnAgregarServicio').click(function () {
    var selectedServicioId = $('#selectServicios').val();
    var selectedServicioText = $('#selectServicios option:selected').text();
    var selectedServicioCosto = $('#inputCostoServicio').val();
    var costoFloat = parseFloat(selectedServicioCosto);

    if (selectedServicioId) {
        if (!serviciosSeleccionados.some(servicio => servicio.id == selectedServicioId)) {

            serviciosSeleccionados.push({
                id: selectedServicioId,
                nombre: selectedServicioText,
                costo: selectedServicioCosto
            });

            var row = '<tr>' +
                '<td>' + selectedServicioText + '</td>' +
                '<td>' + costoFloat + '</td>' +
                '<td><button type="button" class="btn btn-danger btn-sm" onclick="eliminarServicio(this)">Eliminar</button></td>' +
                '</tr>';
            $('#tablaServiciosSeleccionados tbody').append(row);

            actualizarInputServiciosSeleccionados();
            actualizarInfoCosto(); // Llama a la función para actualizar el costo total
        }
    }
});

function eliminarServicio(btn) {
    var rowIndex = $(btn).closest('tr').index();
    serviciosSeleccionados.splice(rowIndex, 1);

    $(btn).closest('tr').remove();

    actualizarInputServiciosSeleccionados();
    actualizarInfoCosto(); // Llama a la función para actualizar el costo total
}

function actualizarInputServiciosSeleccionados() {
    $('#serviciosSeleccionados').val(JSON.stringify(serviciosSeleccionados));
}

function actualizarInfoCosto() {
    var costoServicio = calcularCostosServicio();
    // Actualiza la información en la vista
    $('#totalCostoServicios').text(costoServicio);
}

function calcularCostosServicio() {
    var total = 0;
    serviciosSeleccionados.forEach(servicio =>
        total += parseFloat(servicio.costo)
    );

    return total;
}

$('#selectHabitacion').change(function () {
    var selectedHabitacionId = $(this).val();

    $.ajax({
        url: '/Reservas/ObtenerCostoHabitacion',
        type: 'GET',
        data: { habitacionId: selectedHabitacionId },
        dataType: 'json',
        success: function (data) {
            $('#inputCostoHabitacion').val(data.costo);
            actualizarInfoCosto();
        },
        error: function () {
            console.error('Error al obtener el costo de la habitación.');
        }
    });
});

$('#selectServicio').change(function () {
    var selectedServicioId = $(this).val();

    $.ajax({
        url: '/Reservas/ObtenerCostoServicio',
        type: 'GET',
        data: { servicioId: selectedServicioId },
        dataType: 'json',
        success: function (data) {
            $('#inputCostoServicio').val(data.costo);
            actualizarInfoCosto();
        },
        error: function () {
            console.error('Error al obtener el costo del servicio.');
        }
    });
});

// Dentro del evento para eliminar servicios
$(document).on("click", ".btn-eliminar-servicio", function () {
    $(this).closest("tr").remove();
    calcularCostoTotal();
});
