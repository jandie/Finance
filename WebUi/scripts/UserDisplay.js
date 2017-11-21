$(document).ready(function () {
    if (typeof user === 'undefined') return;

    refreshSummary();
    refreshTransactions();
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
    var glyphClass = transaction.Positive ? 'glyphicon glyphicon-plus positive' :
        'glyphicon glyphicon-minus negative';
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
                                        <p>${description}</p>
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