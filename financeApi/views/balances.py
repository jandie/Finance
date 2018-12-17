from django.contrib.auth.models import User
from rest_framework import status
from rest_framework.response import Response
from rest_framework import permissions
from rest_framework.views import APIView
from django.http import Http404
import datetime

from financeApi.serializers import BalanceSerializer
from financeApi.permissions import IsBalanceOwner
from financeApi.models import Balance
from financeApi.services.overview import get_total_balance
from financeApi.services.balance_history import update_balance_history


class BalanceList(APIView):
    permission_classes = (permissions.IsAuthenticated,)

    def get(self, request, format=None):
        user = User.objects.get(pk=request.user.id)
        serializer = BalanceSerializer(user.balance_set.all(), many=True)
        return Response(serializer.data, status=status.HTTP_200_OK)

    def post(self, request, format=None):
        user = User.objects.get(pk=request.user.id)
        serializer = BalanceSerializer(data=request.data)
        if serializer.is_valid():
            user.balance_set.create(
                name=serializer.data['name'],
                amount=serializer.data['amount']
            )
            update_balance_history(user)
            return Response(BalanceSerializer(user.balance_set.last()).data,
                            status=status.HTTP_201_CREATED)


class BalanceDetail(APIView):
    permission_classes = (permissions.IsAuthenticated,
                          IsBalanceOwner,)

    def get_object(self, pk):
        try:
            obj = Balance.objects.get(pk=pk)
            self.check_object_permissions(self.request, obj)
            return obj
        except Balance.DoesNotExist:
            raise Http404

    def get(self, request, pk, format=None):
        balance = self.get_object(pk)
        serializer = BalanceSerializer(balance)
        return Response(serializer.data)

    def put(self, request, pk, format=None):
        user = User.objects.get(pk=request.user.id)
        balance = self.get_object(pk)
        serializer = BalanceSerializer(balance, data=request.data)
        if serializer.is_valid():
            serializer.save()
            update_balance_history(user)
            return Response(serializer.data)
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

    def delete(self, request, pk, format=None):
        user = User.objects.get(pk=request.user.id)
        balance = self.get_object(pk)
        balance.delete()
        update_balance_history(user)
        return Response(status=status.HTTP_204_NO_CONTENT)
