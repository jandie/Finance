import datetime
from financeApi.services.overview import get_total_balance


def update_balance_history(user):
    todays_balance = get_todays_balance_history(user)

    if todays_balance is not None:
        todays_balance.delete()

    user.balancehistory_set.create(amount=get_total_balance(user))


def get_todays_balance_history(user):
    year = datetime.date.today().year
    month = datetime.date.today().month

    return user.balancehistory_set.filter(date__year=year,
                                          date__month=month).first()


def log_transaction_history(transaction):
    user = get_user_of_transaction(transaction)
    new_th = user.transactionhistory_set \
        .create(transaction_id=transaction.id,
                payment_id=transaction.payment.id,
                payment_name=transaction.payment.name,
                transaction_description=transaction.description,
                transaction_amount=transaction.amount,
                payment_amount=transaction.payment.amount)

    return new_th


def update_transaction_history(transaction):
    th = get_create_transaction_history(transaction)

    th.payment_name = transaction.payment.name
    th.transaction_description = transaction.description
    th.transaction_amount = transaction.amount
    th.payment_amount = transaction.payment.amount
    th.save()

    return th


def deactivate_transaction_history(transaction):
    th = get_create_transaction_history(transaction)

    th.transaction_exists = False
    th.save()

    return th


def get_create_transaction_history(transaction):
    found_th = \
        get_user_of_transaction(transaction) \
            .transactionhistory_set.filter(
            transaction_id=transaction.id).first()

    if found_th is None:
        found_th = log_transaction_history(transaction)

    return found_th


def get_user_of_transaction(transaction):
    return transaction.payment.user


def update_transaction_history_by_payment(payment):
    payment.user.transactionhistory_set.filter(payment_id=payment.id)\
        .update(payment_amount=payment.amount,
                payment_name=payment.name)


def deactivate_transaction_history_by_payment(payment):
    payment.user.transactionhistory_set.filter(payment_id=payment.id)\
        .update(transaction_exists=False)
