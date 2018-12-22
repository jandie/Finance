from django.test import TestCase
from rest_framework.utils import json
from financeApi.tests.utilities import create_user
from rest_framework.test import APIRequestFactory
from financeApi.views.balances import BalanceList


class BalanceTests(TestCase):
    def setUp(self):
        create_user(self)

    def test_create_balance(self):
        factory = APIRequestFactory()
        view = BalanceList.as_view()
        request = factory.post('/balances/',
                               json.dumps({'name': 'test balance',
                                           'amount': 908.36}),
                               HTTP_AUTHORIZATION='JWT ' + self.token,
                               content_type='application/json')
        response = view(request)

        self.assertEqual(response.status_code, 201)
        self.assertEqual(response.data['name'], 'test balance')
        self.assertEqual(response.data['amount'], '908.36')
