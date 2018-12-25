from decimal import Decimal

from django.test import TestCase
from rest_framework.utils import json
from financeApi.tests.utilities import create_user
from rest_framework.test import APIRequestFactory
from financeApi.views.balances import BalanceList, BalanceDetail
from rest_framework.test import APIClient


class BalanceTests(TestCase):
    def setUp(self):
        self.token = None
        self.user = None
        self.user2 = None
        create_user(self)

    def test_get_balance(self):
        balance = self.user.balance_set.create(name='testing balance',
                                               amount=563.93)
        client = APIClient()
        client.credentials(HTTP_AUTHORIZATION='JWT ' + self.token)
        url = '/balances/{}/'.format(balance.id)
        response = client.get(url, content_type='application/json')

        self.assertEqual(response.status_code, 200)
        self.assertEqual(response.data['name'], 'testing balance')
        self.assertEqual(response.data['amount'], '563.93')

    def test_get_balances(self):
        self.user.balance_set.create(name='testing balance',
                                               amount=563.93)
        self.user.balance_set.create(name='testing balance2',
                                               amount=563.03)
        client = APIClient()
        client.credentials(HTTP_AUTHORIZATION='JWT ' + self.token)
        url = '/balances/'
        response = client.get(url, content_type='application/json')

        self.assertEqual(response.status_code, 200)
        self.assertEqual(response.data[0]['name'], 'testing balance')
        self.assertEqual(response.data[0]['amount'], '563.93')
        self.assertEqual(response.data[1]['name'], 'testing balance2')
        self.assertEqual(response.data[1]['amount'], '563.03')
        self.assertEqual(len(response.data), 2)

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

    def test_create_balance_2(self):
        factory = APIRequestFactory()
        view = BalanceList.as_view()
        request = factory.post('/balances/',
                               json.dumps({'name': 'test balance2',
                                           'amount': "900.98"}),
                               HTTP_AUTHORIZATION='JWT ' + self.token,
                               content_type='application/json')
        response = view(request)

        self.assertEqual(response.status_code, 201)
        self.assertEqual(response.data['name'], 'test balance2')
        self.assertEqual(response.data['amount'], '900.98')

    def test_create_balance_3(self):
        factory = APIRequestFactory()
        view = BalanceList.as_view()
        request = factory.post('/balances/',
                               json.dumps({'name': 'test balance2',
                                           'amount': "800.00"}),
                               HTTP_AUTHORIZATION='JWT ' + self.token,
                               content_type='application/json')
        response = view(request)

        self.assertEqual(response.status_code, 201)
        self.assertEqual(response.data['name'],
                         self.user.balance_set.first().name)
        self.assertEqual(Decimal(response.data['amount']),
                         self.user.balance_set.first().amount)
        self.assertEqual(response.data['id'],
                         self.user.balance_set.first().id)

    def test_edit_balance(self):
        balance = self.user.balance_set.create(name='testing balance',
                                               amount=563.93)

        self.assertEqual(self.user.balance_set.first().name,
                         'testing balance')
        self.assertEqual(self.user.balance_set.first().amount,
                         Decimal('563.93'))

        client = APIClient()
        client.credentials(HTTP_AUTHORIZATION='JWT ' + self.token)
        response = client.put('/balances/1/',
                              json.dumps({'id': balance.id,
                                          'name': 'test balance34',
                                          'amount': 108.36}),
                              content_type='application/json')

        self.assertEqual(response.status_code, 200)
        self.assertEqual(response.data['name'], 'test balance34')
        self.assertEqual(response.data['amount'], '108.36')

    def test_edit_balance_security(self):
        balance = self.user2.balance_set.create(name='testing balance',
                                               amount=563.93)
        self.user.balance_set.create(name='testing balance',
                                      amount=563.93)

        self.assertEqual(self.user2.balance_set.first().name,
                         'testing balance')
        self.assertEqual(self.user2.balance_set.first().amount,
                         Decimal('563.93'))

        client = APIClient()
        client.credentials(HTTP_AUTHORIZATION='JWT ' + self.token)
        response = client.put('/balances/1/',
                              json.dumps({'id': balance.id,
                                          'name': 'test balance34',
                                          'amount': 108.36}),
                              content_type='application/json')

        self.assertEqual(response.status_code, 403)

    def test_balance_security(self):
        balance = self.user2.balance_set.create(name='testing balance',
                                               amount=563.93)
        client = APIClient()
        client.credentials(HTTP_AUTHORIZATION='JWT ' + self.token)
        url = '/balances/{}/'.format(balance.id)
        response = client.get(url, content_type='application/json')

        self.assertEqual(response.status_code, 403)

    def test_get_balances_security(self):
        self.user.balance_set.create(name='testing balance',
                                               amount=563.93)
        self.user2.balance_set.create(name='testing balance2',
                                               amount=563.03)
        client = APIClient()
        client.credentials(HTTP_AUTHORIZATION='JWT ' + self.token)
        url = '/balances/'
        response = client.get(url, content_type='application/json')

        self.assertEqual(response.status_code, 200)
        self.assertEqual(response.data[0]['name'], 'testing balance')
        self.assertEqual(response.data[0]['amount'], '563.93')
        self.assertEqual(len(response.data), 1)
