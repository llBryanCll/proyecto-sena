$(document).ready(function () {
    //Peticion a API
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        url: urlBase + '/DataBarras',
        error: function () {
            alert("Ocurrio un error al consultar los datos");
        },
        success: function (data) {
            GraficaBarras(data);
        }
    })
});

function GraficaBarras(data) {
    Highcharts.chart('barras', {
        chart: {
            type: 'column'
        },
        title: {
            text: 'Detalles del flujo'
        },
        subtitle: {
            text: 'Source: Lmour </a>'
        },
        xAxis: {
            type: 'category',
            labels: {
                rotation: -45,
                style: {
                    fontSize: '13px',
                    fontFamily: 'Verdana, sans-serif'
                }
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Cantidad (unidades)'
            }
        },
        legend: {
            enabled: false
        },
        tooltip: {
            pointFormat: 'Apartados mas usados: <b>{point.y:.1f} unidades </b>'
        },
        series: [{
            name: 'Population',
            data: data,
            dataLabels: {
                enabled: true,
                rotation: -90,
                color: '#FFFFFF',
                align: 'right',
                format: '{point.y:.1f}', // one decimal
                y: 10, // 10 pixels down from the top
                style: {
                    fontSize: '13px',
                    fontFamily: 'Verdana, sans-serif'
                }
            }
        }]
    });
}