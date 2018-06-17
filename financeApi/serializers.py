from django.contrib.auth.models import User
from financeApi.models import Balance, Payment, Transaction
from rest_framework import serializers


class UserCreationSerializer(serializers.ModelSerializer):
    class Meta:
        model = User
        fields = ('email', 'password')


class UserSerializer(serializers.ModelSerializer):
    class Meta:
        model = User
        fields = ('id', 'email')


class BalanceSerializer(serializers.ModelSerializer):
    class Meta:
        model = Balance
        fields = ('id', 'name', 'amount')


class PaymentSerializer(serializers.ModelSerializer):
    class Meta:
        model = Payment
        field = ('id', 'name', 'amount', 'outgoing')


class TransactionSerializer(serializers.ModelSerializer):
    class Meta:
        model = Transaction
        field = ('id', 'description', 'amount')
