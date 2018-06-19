import datetime
from django.db.models import Sum, Q


def generate_user_overview(user):
    total_balance = get_total_balance(user)
    to_pay = get_to_pay(user)
    to_get = get_to_get(user)
    end_balance = total_balance - to_pay + to_get

    return {
        "balance": total_balance,
        "endBalance": end_balance,
        "toPay": to_pay,
        "toGet": to_get
    }


def get_total_balance(user):
    return user.balance_set.all().aggregate(Sum('amount'))['amount__sum']


def get_to_pay(user):
    paid = get_paid(user, True)

    total_to_pay = user.payment_set.filter(outgoing=True) \
        .aggregate(Sum('amount'))['amount__sum']

    if total_to_pay is None:
        total_to_pay = 0

    to_pay = total_to_pay - paid

    if to_pay < 0:
        to_pay = 0

    return to_pay


def get_to_get(user):
    gotten = get_paid(user, False)

    total_to_get = user.payment_set.filter(outgoing=False) \
        .aggregate(Sum('amount'))['amount__sum']

    if total_to_get is None:
        total_to_get = 0

    to_get = total_to_get - gotten

    if to_get < 0:
        to_get = 0

    return to_get


def get_paid(user, outgoing):
    year = datetime.date.today().year
    month = datetime.date.today().month
    paid = user.payment_set.filter(outgoing=outgoing).values('name') \
        .annotate(
        amount_paid=Sum('transaction__amount',
                        filter=Q(transaction__created__year=year,
                                 transaction__created__month=month))
    )

    if paid.count() == 0:
        return 0

    return paid[0]['amount_paid']
