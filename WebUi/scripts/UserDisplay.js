$(document).ready(function () {
    refreshUser();

    hideLoading();
    hideMessages();

    checkSession();
    window.checkSessionTimer = setInterval(checkSession, 1 * 60 * 1000);
});

//Logout after 10 minutes of inactivity
$(document).mousemove(function () {
    clearInterval(window.logOutTimer);
    window.logOutTimer = setInterval(logOut, 10 * 60 * 1000); 
});

function refreshUser() {
    if (typeof user === "undefined") return;

    refreshSummary();
    refreshBalances();
    refreshPayments();
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
    $("#TransactionList").append(buildTransactionUi(item, ""));
}

function buildTransactionUi(transaction, divId) {
    var href = `#transaction${divId}${transaction.Id}`;
    var panelId = `transaction${divId}${transaction.Id}`;
    var id = transaction.Id;
    var amount = transaction.Amount.toFixed(2);
    var description = transaction.Description;
    var glyphClass = transaction.Positive ? "glyphicon glyphicon-plus positive" :
        "glyphicon glyphicon-minus negative";
    var content = `<div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                            <a data-toggle="collapse" data-parent="#accordion" href="${href}">
                                <span class="${glyphClass}" aria-hidden="true"></span> ${amount}</a>
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

    refreshTransactions();
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
                                <a class="btn btn-primary btn-sm" role="button" onclick="showManagePayment(${id})">
                                    <span class="glyphicon glyphicon-pencil" aria-hidden="true"></span>
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
    addBalanceOption({ Id: -1, Name: getTranslation(70) });
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
    for (var i = 0; i < language.Translations.length; i++) {
        if (language.Translations[i].Id === id)
            return language.Translations[i].TranslationText;
    }

    return "";
}

function decideProgress(total, amount) {
    var progress = (amount === 0 ? 100 : total / amount * 100).toFixed(0);

    return progress > 100 ? 100 : progress;
}

function hideMessages() {
    $("#ErrorAlert").hide();
    $("#SuccessAlert").hide();
}

function showQuickTransactionModal() {
    fillQuickTransactionOptions();
    $("#QuickTransaction").modal("show");
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
    $("#PaymentModal").modal("hide");
    $("#DeleteTransactionButton").attr("onclick", `removeTransaction(${id})`);
    $("#DeleteTransactionModal").modal("show");
}

function showManageTransaction(id) {
    $("#PaymentModal").modal("hide");
    var transaction = findTransactionById(id);

    $("#TransactionModalLabel").html(transaction.Description);
    $("#EditTransactionDescription").val(transaction.Description);
    $("#EditTransactionAmount").val(transaction.Amount);
    $("#EditTransactionButton").attr("onclick", `changeTransaction(${id})`);

    $("#TransactionModal").modal("show");
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

    return undefined;
}

function showManageBalance(id) {
    var balance = findBalance(id);

    $("#EditBalanceName").val(balance.Name);
    $("#EditBalanceAmount").val(balance.BalanceAmount);
    $("#EditBalanceButton").attr("onclick", `changeBalance(${id})`);
    $("#RemoveBalanceButton").attr("onclick", `showDeleteBalanceConfirmation(${id})`);

    $("#BalanceModal").modal("show");
}

function findBalance(id) {
    for (var i = 0; i < user.Balances.length; i++) {
        if (user.Balances[i].Id === id)
            return user.Balances[i];
    }

    return undefined;
}

function showDeleteBalanceConfirmation(id) {
    $("#BalanceModal").modal("hide");
    $("#DeleteBalanceModalButton").attr("onclick", `removeBalance(${id})`);
    $("#DeleteBalanceModal").modal("show");
}

function showManagePayment(id) {
    var payment = findPayment(id);

    $("#PaymentModalLabel").html(payment.Name);
    $("#EditPaymentName").val(payment.Name);
    $("#EditPaymentAmount").val(payment.Amount);
    $("#EditPaymentButton").attr("onclick", `changePaymentLogic(${id})`);
    $("#RemovePaymentButton").attr("onclick", `showRemovePaymentConfirmation(${id})`);

    $("#PaymentModalTransactionList").empty();

    for (var i = payment.AllTransactions.length - 1; i > -1; i--) {
        $("#PaymentModalTransactionList").append(buildTransactionUi(
            payment.AllTransactions[i], "pmtl"));
    }

    $("#PaymentModal").modal("show");
}

function findPayment(id) {
    var i;

    for (i = 0; i < user.Bills.length; i++) {
        if (user.Bills[i].Id === id)
            return user.Bills[i];
    }

    for (i = 0; i < user.Income.length; i++) {
        if (user.Income[i].Id === id)
            return user.Income[i];
    }

    return undefined;
}

function showRemovePaymentConfirmation(id) {
    $("#PaymentModal").modal("hide");
    $("#DeletePaymentButton").attr("onclick", `removePayment(${id})`);
    $("#DeletePaymentModal").modal("show");
}

function addBillLogic() {
    showLoading();
    $("#AddMonthlyBill").modal("hide");

    var name = $("#AddBillName").val();
    var amount = $("#AddBillAmount").val();

    if (amount === "") {
        showErrorMessage("Invalid number as amount");
        hideLoading();
        return;
    }

    sendPostRequest("../Action/AddMonthlyBill", {
            name: name, amount: amount
        });

    $("#AddBillName").val("");
    $("#AddBillAmount").val("");
}

function addIncomeLogic() {
    showLoading();
    $("#AddMonthlyIncome").modal("hide");

    var name = $("#AddIncomeName").val();
    var amount = $("#AddIncomeAmount").val();

    if (amount === "") {
        showErrorMessage("Invalid number as amount");
        hideLoading();
        return;
    }

    sendPostRequest("../Action/AddMonthlyIncome", {
            name: name, amount: amount
        });

    $("#AddIncomeName").val("");
    $("#AddIncomeAmount").val("");
}

function changePaymentLogic(id) {
    showLoading();
    $("#PaymentModal").modal("hide");

    var name = $("#EditPaymentName").val();
    var amount = $("#EditPaymentAmount").val();

    if (amount === "") {
        showErrorMessage("Invalid number as amount");
        hideLoading();
        return;
    }

    sendPostRequest("../Manage/ChangePayment", {
            id: id, name: name, amount: amount
        });
}

function removePayment(id) {
    showLoading();
    $("#DeletePaymentModal").modal("hide");
    sendPostRequest("../Manage/DeletePayment", {
            id: id
        });
}

function addTransactionLogic() {
    showLoading();
    $("#QuickTransaction").modal("hide");

    var paymentId = $("#PaymentOption").val();
    var description = $("#QuickTransDescription").val();
    var amount = $("#QuickTransAmount").val();
    var balanceId = $("#BalanceOption").val();

    if (amount === "") {
        showErrorMessage("Invalid number as amount");
        hideLoading();
        return;
    }

    sendPostRequest("../Action/AddTransaction", {
        paymentId: paymentId, description: description,
        amount: amount, balanceId: balanceId
    });

    $("#QuickTransDescription").val("");
    $("#QuickTransAmount").val("0.00");
}

function changeTransaction(id) {
    showLoading();
    $("#TransactionModal").modal("toggle");

    var description = $("#EditTransactionDescription").val();
    var amount = $("#EditTransactionAmount").val();

    sendPostRequest("../Manage/ChangeTransaction", {
        id: id, amount: amount, description: description
    });
}

function removeTransaction(id) {
    showLoading();
    $("#DeleteTransactionModal").modal("toggle");

    sendPostRequest("../Manage/DeleteTransaction", {
        id: id
    });
}

function addBalanceLogic() {
    showLoading();
    $("#AddBalanceModal").modal("toggle");

    var name = $("#AddBalanceName").val();
    var balance = $("#AddBalanceBalance").val();

    if (balance === "") {
        showErrorMessage("Invalid number as balance");
        hideLoading();
        return;
    }

    sendPostRequest("../Action/AddBalance", {
        name: name, balance: balance
    });

    $("#QuickTransDescription").val("");
    $("#QuickTransAmount").val("0.00");
}

function changeBalance(id) {
    showLoading();
    $("#BalanceModal").modal("toggle");

    var name = $("#EditBalanceName").val();
    var amount = $("#EditBalanceAmount").val();

    sendPostRequest("../Manage/ChangeBalance", {
        id: id, name: name,
        balanceAmount: amount
    });
}

function removeBalance(id) {
    showLoading();
    $("#DeleteBalanceModal").modal("toggle");
    sendPostRequest("../Manage/DeleteBalance", { id: id });
}

function sendPostRequest(url, data) {
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(data),
        contentType: "application/json",
        success: function (r) {
            var response = JSON.parse(r);
            handleResponse(response);

            if (response.Success) {
                window.user = response.Object;
                refreshUser();
            }
        },
        error: function () {
            showErrorMessage("Something went wrong!");
            hideLoading();
        }
    });
}

function checkSession() {
    if (typeof user === "undefined") return;

    $.ajax({
        type: "POST",
        url: "../Manage/CheckSession",
        contentType: "application/json",
        success: function (r) {
            var response = JSON.parse(r);
            if (!response.Success) {
                logOut();
            }
        },
        error: function () {
            console.log("Error!");
            logOut();
        }
    });
}

function handleResponse(response) {
    hideMessages();
    console.log(response.Message);

    if (response.Success) {
        window.user = response.Object;
        showSuccessMessage(response.Message);
    } else {
        showErrorMessage(response.Message);

        if (response.LogOut)
            logOut();
    }

    hideLoading();
}

function logOut() {
    delete user;

    $.ajax({
        type: "POST",
        url: "../Account/Loguit",
        contentType: "application/json",
        success: function () {
            window.location.reload(true);
        }
    });
}