from rest_framework.test import APIRequestFactory
from django.contrib.auth.models import User, Group
from rest_framework.utils import json
from rest_framework_jwt.views import obtain_jwt_token, verify_jwt_token


def create_user(self):
    self.user = User.objects.create_user(username='tester',
                                         email='tester@pm.me',
                                         password='supersecure')
    self.user2 = User.objects.create_user(username='tester2',
                                          email='tester2@pm.me',
                                          password='supersecure')
    view = obtain_jwt_token
    factory = APIRequestFactory()
    request = factory.post('/api-token-auth',
                           json.dumps({
                               'username': 'tester',
                               'password': 'supersecure'
                           }), content_type='application/json')
    response = view(request)
    self.token = response.data['token']
    self.assertEquals(response.status_code, 200)
