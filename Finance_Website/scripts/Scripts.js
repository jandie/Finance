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