const fecha = new Date();
var formato = 'dd/mm/yyyy';

const map = {
    dd: (fecha.getDate() < 10 ? '0' : '') + fecha.getDate(),
    mm: (fecha.getMonth() + 1 < 10 ? '0' : '') + (fecha.getMonth() + 1),
    yy: fecha.getFullYear().toString().slice(-2),
    yyyy: fecha.getFullYear()
}

var fechaHoy = formato.replace(/dd|mm|yyyy/gi, matched => map[matched])

$('#inputfechaabono').val(fechaHoy)