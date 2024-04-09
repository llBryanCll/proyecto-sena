const fecha = new Date();
var formato = 'dd/mm/yyyy';

const map = {
    dd: (fecha.getDate() < 10 ? '0' : '') + fecha.getDate(),
    mm: (fecha.getMonth() + 1 < 10 ? '0' : '') + (fecha.getMonth() + 1),
    yyyy: fecha.getFullYear()
}

var fechaHoy = formato.replace(/dd|mm|yyyy/gi, matched => map[matched]);

$('#fechaHoy').val(fechaHoy);

$(function () {
    $("#fechaIngreso").datepicker();
    $("#fechaSalida").datepicker();
});

$(function () {
    // Inicializa el Datepicker para la fecha de ingreso
    $("#fechaIngreso").datepicker({
        minDate: 0, // Establece la fecha mínima como la fecha actual
        dateFormat: "yy-mm-dd", // Formato de fecha
        onSelect: function (selectedDate) {
            var minDate = new Date(selectedDate); // Fecha de ingreso seleccionada
            var today = new Date(); // Fecha actual
            // Comprueba si la fecha de ingreso seleccionada es anterior a la fecha actual
            if (minDate < today) {
                // Establece la fecha de ingreso seleccionada como la fecha actual
                $(this).datepicker("setDate", today);
            }
            // Establece la fecha mínima de salida como la fecha de ingreso seleccionada
            $("#fechaSalida").datepicker("option", "minDate", minDate);
        }
    });

    // Inicializa el Datepicker para la fecha de salida
    $("#fechaSalida").datepicker({
        minDate: 0, // Establece la fecha mínima como la fecha actual
        dateFormat: "yy-mm-dd", // Formato de fecha
        onSelect: function (selectedDate) {
            var maxDate = new Date(selectedDate); // Fecha de salida seleccionada
            var minDate = $("#fechaIngreso").datepicker("getDate"); // Fecha de ingreso seleccionada
            // Comprueba si la fecha de salida seleccionada es anterior a la fecha de ingreso
            if (maxDate < minDate) {
                // Establece la fecha de salida seleccionada como la fecha de ingreso seleccionada
                $(this).datepicker("setDate", minDate);
            }
        }
    });
});

var paqueteSeleccionado = [];
var serviciosSeleccionados = [];
$('#selectPaquete').change(function () {

    var selectedPaqueteId = $(this).val();

    $.ajax({
        url: '/Reservas/ObtenerCostoPaquete',
        type: 'GET',
        data: { paqueteId: selectedPaqueteId },
        dataType: 'json',
        success: function (data) {
            $('#inputCostoPaquete').val(data.costo)

            var selectedPaqueteCosto = $('#inputCostoPaquete').val()

            paqueteSeleccionado = [];

            paqueteSeleccionado.push({
                id: selectedPaqueteId,
                costo: selectedPaqueteCosto
            });

            actualizarInputPaqueteSeleccionado();
            actualizarInfoCosto();
        },
        error: function () {
            console.error('Error al obtener el costo del paquete.')
        }
    })


})

function actualizarInputPaqueteSeleccionado() {
    $('#paqueteSeleccionado').val(JSON.stringify(paqueteSeleccionado));
}

$('#selectServicios').change(function () {
    var selectedServicioId = $(this).val();

    $.ajax({
        url: '/Reservas/ObtenerCostoServicio',
        type: 'GET',
        data: { servicioId: selectedServicioId },
        dataType: 'json',
        success: function (data) {
            $('#inputCostoServicio').val(data.costo)
        },
        error: function () {
            console.error('Error al obtener el costo del servicio.')
        }
    })
});

$('#btnAgregarServicio').click(function () {
    var selectedServicioId = $('#selectServicios').val();
    var selectedServicioText = $('#selectServicios option:selected').text();
    var selectedServicioCosto = $('#inputCostoServicio').val();
    var costoFloat = parseFloat(selectedServicioCosto)

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
            actualizarInfoCosto();
        }
    }

});

function eliminarServicio(btn) {

    var rowIndex = $(btn).closest('tr').index();
    serviciosSeleccionados.splice(rowIndex, 1);

    $(btn).closest('tr').remove();

    actualizarInputServiciosSeleccionados();
    actualizarInfoCosto();
}

function actualizarInputServiciosSeleccionados() {
    $('#serviciosSeleccionados').val(JSON.stringify(serviciosSeleccionados));
}

function actualizarInfoCosto() {

    var costoPaquete = $('#inputCostoPaquete').val() || 0;
    var costoPaqueteFloat = parseFloat(costoPaquete);
    var costoServicio = calcularCostosServicio();
    var subTotal = costoPaqueteFloat + costoServicio;
    var descuento = parseFloat($('#inputDescuento').val()) || 0;
    var valorDescuento = subTotal * (1 - (descuento / 100));
    var iva = Math.round((valorDescuento) * 0.19);
    var total = Math.round(valorDescuento + iva);

    $('#inputSubTotal').val(subTotal);
    $('#inputIva').val(iva);
    $('#inputTotal').val(total);
}

function calcularCostosServicio() {
    var total = 0;
    serviciosSeleccionados.forEach(servicio =>
        total += parseFloat(servicio.costo)
    )

    return total;
}

function formatoDescuento(input) {

    let value = input.value.replace(/\D/g, '')

    if (value === '') {
        input.value = '';
    } else {
        value = Math.min(Math.max(parseInt(value), 0), 100);
        input.value = value
    }
}
