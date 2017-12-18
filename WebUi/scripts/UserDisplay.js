$(document).ready(function () {
    refreshUser();

    hideLoading();
    hideMessages();
});

function refreshUser() {
    if (typeof user === "undefined") return;

    refreshSummary();
    refreshTransactions();
    refreshBalances();
    refreshPayments();
    fillQuickTransactionOptions();

    hideLoading();
    hideMessages();
}

function refreshSummary() {
    $("#UserTotalBalance").text(user.TotalBalance.toFixed(2));
    $("#UserPrediction").text(user.Prediction.toFixed(2));
    $("#UserToPay").text(user.ToPay.toFixed(2));
    $("#UserToGet").text(user.ToGet.toFixed(2));

    drawGraph();
}

function refreshTransactions() {
    $("#TransactionList").empty();
    user.TransactionList.forEach(addTransactionUi);
}

function addTransactionUi(item) {
    $("#TransactionList").append(buildTransactionUi(item));
}

function buildTransactionUi(transaction) {
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
                                            <a class="btn btn-primary btn-sm" role="button" onclick="showManageTransaction(${id})">
                                                <span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></a>
                                            <a class="btn btn-danger btn-sm" role="button" onclick="showDeleteTransactionConfirmation(${id})">
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
                                <a class="btn btn-primary btn-sm" role="button" onclick="showManageBalance(${id})">
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

function fillQuickTransactionOptions() {
    $("#PaymentOption").empty();
    $("#BalanceOption").empty();
    user.Bills.forEach(addBillOption);
    user.Income.forEach(addIncomeOption);
    user.Balances.forEach(addBalanceOption);
}

function addBillOption(bill) {
    var content = `<option value="${bill.Id}" class="negative">${bill.Name}</option>`;

    $("#PaymentOption").append(content);
}

function addIncomeOption(income) {
    var content = `<option value="${income.Id}" class="positive">${income.Name}</option>`;

    $("#PaymentOption").append(content);
}

function addBalanceOption(balance) {
    var content = `<option value="${balance.Id}">${balance.Name}</option>`;

    $("#BalanceOption").append(content);
}

function getTranslation(id) {
    return language.Translations[id - 1].TranslationText;
}

function decideProgress(total, amount) {
    return (amount === 0 ? 100 : total / amount * 100).toFixed(0);
}

function hideMessages() {
    $("#ErrorAlert").hide();
    $("#SuccessAlert").hide();
}

function showSuccessMessage(message) {
    hideMessages();
    $("#SuccessMessage").empty();
    $("#SuccessMessage").append("<strong>Success!</strong> " + message);

    $("#SuccessAlert").show();
}

function showErrorMessage(message) {
    hideMessages();
    $("#ErrorMessage").empty();
    $("#ErrorMessage").append("<strong>Error!</strong> " + message);

    $("#ErrorAlert").show();
}

function showDeleteTransactionConfirmation(id) {
    $("#DeleteTransactionButton").attr("onclick", `removeTransaction(${id})`);
    $("#DeleteTransactionModal").modal("toggle");
}

function showManageTransaction(id) {
    var transaction = findTransactionById(id);

    $("#EditTransactionDescription").val(transaction.Description);
    $("#EditTransactionAmount").val(transaction.Amount);
    $("#EditTransactionButton").attr("onclick", `changeTransaction(${id})`);

    $("#TransactionModal").modal("toggle");
}

function findTransactionById(id) {
    var t;
    var i;

    for (i = 0; i < user.Bills.length; i++) {
        t = findTransaction(user.Bills[i].AllTransactions, id);

        if (!(typeof t === "undefined"))
            return t;
    }

    for (i = 0; i < user.Income.length; i++) {
        t = findTransaction(user.Income[i].AllTransactions, id);

        if (!(typeof t === "undefined"))
            return t;
    }

    return undefined;
}

function findTransaction(transactions, id) {
    for (var i = 0; i < transactions.length; i++) {
        if (transactions[i].Id === id)
            return transactions[i];
    }
}

function showManageBalance(id) {
    var balance = findBalance(id);

    $("#EditBalanceName").val(balance.Name);
    $("#EditBalanceAmount").val(balance.BalanceAmount);
    $("#EditBalanceButton").attr("onclick", `changeBalance(${id})`);
    $("#RemoveBalanceButton").attr("onclick", `showDeleteBalanceConfirmation(${id})`);

    $("#BalanceModal").modal("toggle");
}

function findBalance(id) {
    for (var i = 0; i < user.Balances.length; i++) {
        if (user.Balances[i].Id === id)
            return user.Balances[i];
    }

    return undefined;
}

function showDeleteBalanceConfirmation(id) {
    $("#BalanceModal").modal("toggle");
    $("#DeleteBalanceModalButton").attr("onclick", `removeBalance(${id})`);
    $("#DeleteBalanceModal").modal("toggle");
}

function addTransactionLogic() {
    showLoading();
    $("#QuickTransaction").modal("toggle");

    var paymentId = $("#PaymentOption").val();
    var description = $("#QuickTransDescription").val();
    var amount = $("#QuickTransAmount").val();
    var balanceId = $("#BalanceOption").val();

    if (amount === "") {
        showErrorMessage("Invalid number as amount");
        hideLoading();
        return;
    }

    $.ajax({
        type: "POST",
        url: "../Action/AddTransaction",
        data: JSON.stringify({
            paymentId: paymentId, description: description,
            amount: amount, balanceId: balanceId
        }),
        contentType: "application/json",
        success: function (r) {
            var response = JSON.parse(r);
            handleResponse(response);

            if (response.Success) {
                user = response.Object;
                refreshTransactions();
                refreshSummary();

                $("#QuickTransDescription").val("");
                $("#QuickTransAmount").val("0.00");
            }
        },
        error: function () {
            showErrorMessage("Something went wrong!");
            hideLoading();
        }
    });
}

function removeTransaction(id) {
    showLoading();
    $("#DeleteTransactionModal").modal("toggle");
    $.ajax({
        type: "POST",
        url: "../Manage/DeleteTransaction",
        data: JSON.stringify({
            id: id
        }),
        contentType: "application/json",
        success: function (r) {
            var response = JSON.parse(r);
            handleResponse(response);

            if (response.Success) {
                user = response.Object;
                refreshTransactions();
                refreshSummary();
            }
        },
        error: function () {
            showErrorMessage("Something went wrong!");
            hideLoading();
        }
    });
}

function changeTransaction(id) {
    showLoading();
    $("#TransactionModal").modal("toggle");

    var description = $("#EditTransactionDescription").val();
    var amount = $("#EditTransactionAmount").val();
    
    $.ajax({
        type: "POST",
        url: "../Manage/ChangeTransaction",
        data: JSON.stringify({
            id: id, amount: amount, description: description
        }),
        contentType: "application/json",
        success: function (r) {
            var response = JSON.parse(r);
            handleResponse(response);

            if (response.Success) {
                user = response.Object;
                refreshTransactions();
                refreshSummary();
            }
        },
        error: function () {
            showErrorMessage("Something went wrong!");
            hideLoading();
        }
    });
}

function changeBalance(id) {
    showLoading();
    $("#BalanceModal").modal("toggle");

    var name = $("#EditBalanceName").val();
    var amount = $("#EditBalanceAmount").val();

    $.ajax({
        type: "POST",
        url: "../Manage/ChangeBalance",
        data: JSON.stringify({
            id: id, name: name, balanceAmount: amount
        }),
        contentType: "application/json",
        success: function (r) {
            var response = JSON.parse(r);
            handleResponse(response);

            if (response.Success) {
                user = response.Object;
                refreshBalances();
                refreshSummary();
            }
        },
        error: function () {
            showErrorMessage("Something went wrong!");
            hideLoading();
        }
    });
}

function removeBalance(id) {
    showLoading();
    $("#DeleteBalanceModal").modal("toggle");

    $.ajax({
        type: "POST",
        url: "../Manage/DeleteBalance",
        data: JSON.stringify({
            id: id
        }),
        contentType: "application/json",
        success: function (r) {
            var response = JSON.parse(r);
            handleResponse(response);

            if (response.Success) {
                user = response.Object;
                refreshBalances();
                refreshSummary();
            }
        },
        error: function () {
            showErrorMessage("Something went wrong!");
            hideLoading();
        }
    });
}

function handleResponse(response) {
    hideMessages();
    console.log(response.Message);

    if (response.Success) {
        user = response.Object;
        showSuccessMessage(response.Message);
    } else {
        showErrorMessage(response.Message);
    }

    hideLoading();
}

function logOut() {
    $.ajax({
        type: "POST",
        url: "../Account/Loguit",
        contentType: "application/json",
        success: function () {
            window.location.reload(true);
        }
    });
}