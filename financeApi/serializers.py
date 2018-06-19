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
    transactions = serializers.ListField(child=TransactionSerializer(),
                                         source='monthly_transactions')
    paid = serializers.CharField(source='amount_paid.amount__sum')

    class Meta:
        model = Payment
        fields = ('id', 'name', 'amount', 'paid', 'transactions', 'outgoing')


class OverviewSerializer(serializers.Serializer):
    balance = serializers.DecimalField(max_digits=12, decimal_places=2)
    endBalance = serializers.DecimalField(max_digits=12, decimal_places=2)
    toPay = serializers.DecimalField(max_digits=12, decimal_places=2)
    toGet = serializers.DecimalField(max_digits=12, decimal_places=2)

    def update(self, instance, validated_data):
        pass

    def create(self, validated_data):
        pass
