from django.db import models
from django.contrib.auth.models import User


class Balance(models.Model):
    user = models.ForeignKey(User, on_delete=models.CASCADE)

    name = models.CharField(max_length=256)
    amount = models.DecimalField(max_digits=12, decimal_places=2)


class Payment(models.Model):
    user = models.ForeignKey(User, on_delete=models.CASCADE)

    name = models.CharField(max_length=256)
    amount = models.DecimalField(max_digits=12, decimal_places=2)
    outgoing = models.BooleanField(default=True)


class Transaction(models.Model):
    payment = models.ForeignKey(Payment, on_delete=models.CASCADE)

    description = models.CharField(max_length=1024)
    amount = models.DecimalField(max_digits=12, decimal_places=2)
