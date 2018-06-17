from django.contrib.auth.models import User
from financeApi.models import Balance, Payment, Transaction
from rest_framework import serializers


class UserSerializer(serializers.ModelSerializer):
    password = serializers.CharField(write_only=True)

    class Meta:
        model = User
        fields = ('id', 'username', 'password')

    def create(self, validated_data):
        user = super(UserSerializer, self).create(validated_data)
        user.set_password(validated_data['password'])
        user.save()
        return user


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
