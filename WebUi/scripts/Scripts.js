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

function convertGraphData() {
    if (typeof user === 'undefined') return;

    var size = user.BalanceHistories.length;
    var data = new Array(size);

    for (var i = 0; i < size; i++) {
        data[i] = new Array(2);
        data[i][1] = user.BalanceHistories[i].Amount;
        data[i][0] = i;
    }

    window.graphData = [{
            data: data,
            color: '#71c73e'
        }
    ];
}

function drawGraph() {
    convertGraphData();
    if (typeof graphData === 'undefined') return;

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

function onresize() {
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