from django.contrib.auth.models import User
from rest_framework import status
from rest_framework.decorators import api_view
from rest_framework.response import Response
from rest_framework_jwt.settings import api_settings

from financeApi.serializers import UserSerializer


@api_view(['POST'])
def users_creation(request):
    jwt_payload_handler = api_settings.JWT_PAYLOAD_HANDLER
    jwt_encode_handler = api_settings.JWT_ENCODE_HANDLER

    if request.method == 'POST':
        serializer = UserSerializer(data=request.data)
        if not serializer.is_valid():
            return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)
        serializer.save()
        user = serializer.data
        print(user['username'])
        user = User.objects.get(username=user['username'])
        token = jwt_encode_handler(jwt_payload_handler(user))
        return Response({
            'token': token
        }, status=status.HTTP_201_CREATED )
