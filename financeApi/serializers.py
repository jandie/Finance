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
    id = serializers.PrimaryKeyRelatedField(read_only=True)

    class Meta:
        model = Balance
        fields = ('id', 'name', 'amount')


class TransactionSerializer(serializers.ModelSerializer):
    id = serializers.PrimaryKeyRelatedField(read_only=True)

    class Meta:
        model = Transaction
        fields = ('id', 'description', 'amount')


class PaymentSerializer(serializers.ModelSerializer):
    id = serializers.PrimaryKeyRelatedField(read_only=True)
    monthly_transactions = serializers.ListField(child=TransactionSerializer())

    class Meta:
        model = Payment
        fields = ('id', 'name', 'amount', 'amount_paid', 'monthly_transactions', 'outgoing')
