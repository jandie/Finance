$(document).ready(function () {
    $("#AddBalanceModal").on("shown.bs.modal", function () {
        $("#name").focus();
    });
    $("#AddMonthlyBill").on("shown.bs.modal", function () {
        $("#billName").focus();
    });
    $("#AddMonthlyIncome").on("shown.bs.modal", function () {
        $("#incomeName").focus();
    });
});

var drawGraph = function () {
    if (typeof graphData == 'undefined') return;

    // Lines Graph
    $.plot('#graph-lines', graphData, {
        series: {
            points: {
                show: false
            },
            lines: {
                show: true
            },
            shadowSize: 5
        },
        grid: {
            color: '#646464',
            borderColor: 'transparent',
            borderWidth: 20,
            hoverable: true
        },
        xaxis: {
            tickColor: 'transparent',
            show: false
        }
    });
}

var onresize = function (e) {
    drawGraph();
}

$(document).ready(function () {
    drawGraph();
    
    window.addEventListener("resize", onresize);
});

function showLoading() {
    $("#loader").show();
}

function hideLoading() {
    $("#loader").hide();
}