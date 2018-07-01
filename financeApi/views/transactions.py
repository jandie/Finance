from django.contrib.auth.models import User
from rest_framework import status
from rest_framework.response import Response
from rest_framework import permissions
from rest_framework.views import APIView
from django.http import Http404

from financeApi.serializers import TransactionSerializer
from financeApi.permissions import IsTransactionOwner, IsPaymentOwner
from financeApi.models import Transaction, Payment
import datetime


class TransactionList(APIView):
    permission_classes = (permissions.IsAuthenticated,)

    def get(self, request, format=None):
        user = User.objects.get(pk=request.user.id)
        year = datetime.date.today().year
        month = datetime.date.today().month
        transactions = Transaction.objects.filter(payment__user__id=user.id,
                                       created__year=year,
                                       created__month=month)
        transaction_serializer = \
            TransactionSerializer(transactions, many=True)
        return Response(transaction_serializer.data, status=status.HTTP_200_OK)


class PaymentTransactionList(APIView):
    permission_classes = (permissions.IsAuthenticated,
                          IsPaymentOwner)

    def get_object(self, pk):
        try:
            obj = Payment.objects.get(pk=pk)
            self.check_object_permissions(self.request, obj)
            return obj
        except Payment.DoesNotExist:
            raise Http404

    def get(self, request, pk, format=None):
        payment = self.get_object(pk)
        serializer = TransactionSerializer(payment.transaction_set.all(), many=True)
        return Response(serializer.data, status=status.HTTP_200_OK)

    def post(self, request, pk, format=None):
        payment = self.get_object(pk)
        serializer = TransactionSerializer(data=request.data)
        if serializer.is_valid():
            payment.transaction_set.create(
                description=serializer.data['description'],
                amount=serializer.data['amount']
            )
            return Response(TransactionSerializer(payment.transaction_set.last()).data,
                            status=status.HTTP_201_CREATED)


class TransactionDetail(APIView):
    permission_classes = (permissions.IsAuthenticated,
                          IsTransactionOwner,)

    def get_object(self, pk):
        try:
            obj = Transaction.objects.get(pk=pk)
            self.check_object_permissions(self.request, obj)
            return obj
        except Transaction.DoesNotExist:
            raise Http404

    def get(self, request, pk, format=None):
        transaction = self.get_object(pk)
        serializer = TransactionSerializer(transaction)
        return Response(serializer.data)

    def put(self, request, pk, format=None):
        transaction = self.get_object(pk)
        serializer = TransactionSerializer(transaction, data=request.data)
        if serializer.is_valid():
            serializer.save()
            return Response(serializer.data)
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

    def delete(self, request, pk, format=None):
        transaction = self.get_object(pk)
        transaction.delete()
        return Response(status=status.HTTP_204_NO_CONTENT)
