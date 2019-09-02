from django.test import TestCase
from rest_framework.test import APIRequestFactory
from rest_framework.utils import json
from rest_framework_jwt.views import verify_jwt_token

from financeApi.tests.utilities import create_user


class AuthTests(TestCase):
    def setUp(self):
        """ Creates user to login with. """
        create_user(self)

    def test_verify_token(self):
        """ Tests if the token is verified correctly
        when correct token is given """
        factory = APIRequestFactory()
        view = verify_jwt_token
        request = factory.post('/api-token-verify',
                               json.dumps({'token': self.token}),
                               content_type='application/json')
        response = view(request)
        assert response.data['token'] == self.token
        assert response.status_code == 200

    def test_verify_wrong_token(self):
        """ Tests if the token is verified correctly
        when wrong token is given """
        factory = APIRequestFactory()
        view = verify_jwt_token
        request = factory.post('/api-token-verify',
                               json.dumps({'token': 'randomtokenthatisincorrect'}),
                               content_type='application/json')
        response = view(request)
        assert response.status_code == 400
