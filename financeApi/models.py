import datetime
from django.db import models
from django.db.models import Sum
from django.contrib.auth.models import User


class Balance(models.Model):
    user = models.ForeignKey(User, on_delete=models.CASCADE)

    name = models.CharField(max_length=256)
    amount = models.DecimalField(max_digits=12, decimal_places=2)

    def __str__(self):
        return self.name


class BalanceHistory(models.Model):
    user = models.ForeignKey(User, on_delete=models.CASCADE)

    amount = models.DecimalField(max_digits=12, decimal_places=2)
    date = models.DateField(db_index=True, auto_now_add=True)

    def __str__(self):
        return self.amount


class Payment(models.Model):
    user = models.ForeignKey(User, on_delete=models.CASCADE)

    name = models.CharField(max_length=256)
    amount = models.DecimalField(max_digits=12, decimal_places=2)
    outgoing = models.BooleanField(default=True)

    @property
    def amount_paid(self):
        year = datetime.date.today().year
        month = datetime.date.today().month
        paid = self.transaction_set.filter(created__year=year,
                                           created__month=month) \
            .aggregate(Sum('amount'))

        if paid['amount__sum'] is None:
            paid['amount__sum'] = 0

        return paid

    @property
    def monthly_transactions(self):
        year = datetime.date.today().year
        month = datetime.date.today().month
        transactions = self.transaction_set.filter(created__year=year,
                                                   created__month=month)

        return transactions

    def __str__(self):
        return self.name


class Transaction(models.Model):
    payment = models.ForeignKey(Payment, on_delete=models.CASCADE)

    description = models.CharField(max_length=1024)
    amount = models.DecimalField(max_digits=12, decimal_places=2)
    created = models.DateField(default=datetime.date.today, db_index=True)

    @property
    def outgoing(self):
        return self.payment.outgoing;

    def __str__(self):
        return self.description
