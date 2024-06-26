﻿const fecha = new Date();
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
    // Función para formatear la fecha en formato ISO (yyyy-mm-dd)
    function formatDate(date) {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2)
            month = '0' + month;
        if (day.length < 2)
            day = '0' + day;

        return [year, month, day].join('-');
    }

    // Función para obtener la fecha actual en formato ISO
    function getCurrentDate() {
        var today = new Date();
        return formatDate(today);
    }

    // Inicializa el Datepicker para la fecha de ingreso
    $("#fechaIngreso").change(function () {
        var selectedDate = $(this).val();
        var minDate = new Date(selectedDate);
        var today = new Date();
        if (minDate < today) {
            $(this).val(getCurrentDate());
        }
        $("#fechaSalida").attr("min", selectedDate);
    });

    // Inicializa el Datepicker para la fecha de salida
    $("#fechaSalida").change(function () {
        var maxDate = new Date($(this).val());
        var minDate = new Date($("#fechaIngreso").val());
        if (maxDate < minDate) {
            $(this).val($("#fechaIngreso").val());
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
    $('#inputMontoTotal').val(total);
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
