from django.contrib.auth.models import User
from rest_framework import status
from rest_framework.response import Response
from rest_framework import permissions
from rest_framework.views import APIView
from django.http import Http404

from financeApi.serializers import PaymentSerializer
from financeApi.permissions import IsPaymentOwner
from financeApi.models import Payment


class PaymentList(APIView):
    permission_classes = (permissions.IsAuthenticated,)

    def get(self, request, format=None):
        user = User.objects.get(pk=request.user.id)
        payment_serializer = PaymentSerializer(user.payment_set.all(), many=True)
        return Response(payment_serializer.data, status=status.HTTP_200_OK)

    def post(self, request, format=None):
        user = User.objects.get(pk=request.user.id)
        serializer = PaymentSerializer(data=request.data)
        if serializer.is_valid():
            user.payment_set.create(
                name=serializer.data['name'],
                amount=serializer.data['amount'],
                outgoing=serializer.data['outgoing']
            )
            payment_new = PaymentSerializer(user.payment_set.last())
            return Response(payment_new.data, status=status.HTTP_201_CREATED)


class PaymentDetail(APIView):
    permission_classes = (permissions.IsAuthenticated,
                          IsPaymentOwner,)

    def get_object(self, pk):
        try:
            obj = Payment.objects.get(pk=pk)
            self.check_object_permissions(self.request, obj)
            return obj
        except Payment.DoesNotExist:
            raise Http404

    def get(self, request, pk, format=None):
        payment = self.get_object(pk)
        serializer = PaymentSerializer(payment)
        return Response(serializer.data)

    def put(self, request, pk, format=None):
        payment = self.get_object(pk)
        serializer = PaymentSerializer(payment, data=request.data)
        if serializer.is_valid():
            serializer.save()
            return Response(serializer.data)
        return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

    def delete(self, request, pk, format=None):
        payment = self.get_object(pk)
        payment.delete()
        return Response(status=status.HTTP_204_NO_CONTENT)