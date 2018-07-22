from django.contrib.auth.models import User
from rest_framework import status
from rest_framework.decorators import api_view
from rest_framework.response import Response
from rest_framework_jwt.settings import api_settings
from rest_framework import permissions
from rest_framework.views import APIView

from financeApi.serializers import UserSerializer, OverviewSerializer
from financeApi.services.overview import generate_user_overview


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
        user = User.objects.get(username=user['username'])
        token = jwt_encode_handler(jwt_payload_handler(user))
        return Response({
            'token': token
        }, status=status.HTTP_201_CREATED)


class UserOverview(APIView):
    permission_classes = (permissions.IsAuthenticated,)

    def get(self, request, format=None):
        user = request.user
        serializer = OverviewSerializer(generate_user_overview(user))
        return Response(serializer.data, status=status.HTTP_200_OK)
