$(document).ready(function () {
    if (typeof user === 'undefined') return;

    RefreshSummary();
});

function RefreshSummary() {
    $("#UserTotalBalance").text(user.TotalBalance.toFixed(2));
    $("#UserPrediction").text(user.Prediction.toFixed(2));
    $("#UserToPay").text(user.ToPay.toFixed(2));
    $("#UserToGet").text(user.ToGet.toFixed(2));
}