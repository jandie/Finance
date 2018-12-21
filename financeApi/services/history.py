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
        .create(transactionId=transaction.id,
                payment_id=transaction.payment.id,
                name=transaction.payment.name,
                description=transaction.description,
                amount=transaction.amount)

    return new_th


def update_transaction_history(transaction):
    th = get_user_of_transaction(transaction) \
        .transactionhistory_set.get(transaction_id=transaction.id)

    th.name = transaction.payment.name
    th.description = transaction.description
    th.amount = transaction.amount
    th.save()

    return th


def deactivate_transaction_history(transaction):
    th = get_user_of_transaction(transaction) \
        .transactionhistory_set.get(transaction_id=transaction.id)

    th.exists = False
    th.save()

    return th


def get_transaction_history(transaction):
    found_th = \
        get_user_of_transaction(transaction) \
            .transactionhistory_set.get(
            transaction_id=transaction.id)

    if found_th is None:
        found_th = log_transaction_history(transaction)

    return found_th


def get_user_of_transaction(transaction):
    return transaction.payment.user

# TODO: Update transaction history name when payment changes
# Example User.objects.filter(id=data['id']).update(email=data['email'], phone=data['phone'])
