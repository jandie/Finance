$(document).ready(function () {
    if (typeof user === "undefined") return;

    refreshSummary();
    refreshTransactions();
    refreshBalances();
    refreshPayments();
});

function refreshSummary() {
    $("#UserTotalBalance").text(user.TotalBalance.toFixed(2));
    $("#UserPrediction").text(user.Prediction.toFixed(2));
    $("#UserToPay").text(user.ToPay.toFixed(2));
    $("#UserToGet").text(user.ToGet.toFixed(2));
}

function refreshTransactions() {
    $("#TransactionList").empty();
    user.TransactionList.forEach(addTransactionUi);
}

function addTransactionUi(item) {
    $("#TransactionList").append(buildTransactionUi(item));
}

function buildTransactionUi(transaction) {
    console.log(`${transaction.Description} - ${transaction.Amount.toFixed(2)}`);

    var href = `#transaction${transaction.Id}`;
    var panelId = `transaction${transaction.Id}`;
    var id = transaction.Id;
    var amount = transaction.Amount.toFixed(2);
    var description = transaction.Description;
    var glyphClass = transaction.Positive ? "glyphicon glyphicon-plus positive" :
        "glyphicon glyphicon-minus negative";
    var content = `<div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                            <a data-toggle="collapse" data-parent="#accordion" href="${href}">
                                <span class="${glyphClass}" aria-hidden="true"></span>${amount}</a>
                            </h4>
                        </div>
                        <div id="${panelId}" class="panel-collapse collapse">
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-10">
                                        <p><strong>${getTranslation(18)}</strong> ${description}</p>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 pull-right">
                                        <div class="btn-group">
                                            <a class="btn btn-primary btn-sm" role="button" href="../Manage/Transaction?id=${id}">
                                                <span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></a>
                                            <a class="btn btn-danger btn-sm" role="button" href="../Manage/DeleteTransaction?id=${id}&quick=true">
                                                <span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>`;

    return content;
}

function refreshBalances() {
    $("#UserBalances").empty();
    user.Balances.forEach(addBalanceUi);
}

function addBalanceUi(balance) {
    $("#UserBalances").append(buildBalanceUi(balance));
}

function buildBalanceUi(balance) {
    var id = balance.Id;
    var name = balance.Name;
    var amount = balance.BalanceAmount.toFixed(2);
    var content = `<tr>
                        <td>${name}</td>
                        <td>${amount}</td>
                        <td>
                            <div class="btn-group">
                                <a class="btn btn-primary btn-sm" role="button" href="~/Manage/Balance?id=${id}&lastTab=1">
                                    <span class="glyphicon glyphicon-pencil" aria-hidden="true"></span>
                                </a>
                            </div>
                        </td>
                    </tr>`;

    return content;
}

function refreshPayments() {
    $("#UserBills").empty();
    $("#UserIncome").empty();
    user.Bills.forEach(addBillUi);
    user.Income.forEach(addIncomeUi);
}

function addBillUi(bill) {
    $("#UserBills").append(buildPaymentUi(bill));
}

function addIncomeUi(income) {
    $("#UserIncome").append(buildPaymentUi(income));
}

function buildPaymentUi(payment) {
    var id = payment.Id;
    var name = payment.Name;
    var total = payment.Total;
    var amount = payment.Amount;
    var progress = decideProgress(total, amount);
    var progressString = `${progress}%`;

    var content = `<tr>
                        <td>${name}</td>
                        <td>${amount.toFixed(2)}</td>
                        <td>${total.toFixed(2)}</td>
                        <td>
                            <div class="btn-group-vertical">
                                <a class="btn btn-primary btn-sm" role="button" href="../Manage/Payment?id=${id}&lastTab=2">
                                    <span class="glyphicon glyphicon-pencil" aria-hidden="true"></span>
                                </a>
                                <a class="btn btn-success btn-sm" role="button" href="../Action/Transaction?paymentId=${id}&lastTab=2">
                                    <span class="glyphicon glyphicon-ok-circle" aria-hidden="true"></span>
                                </a>
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="4">
                            <div class="progress" style="margin-bottom: 0px;">
                                <div class="progress-bar progress-bar-striped" role="progressbar" aria-valuenow="${progress}" aria-valuemin="0" aria-valuemax="100" style="width: ${progressString}">
                                    ${progress} %
                                </div>
                            </div>
                        </td>
                    </tr>`;

    return content;
}

function getTranslation(id) {
    return language.Translations[id - 1].TranslationText;
}

function decideProgress(total, amount) {
    return (amount === 0 ? 100 : total / amount * 100).toFixed(0);
}