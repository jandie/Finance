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
