from django.conf.urls import url
from rest_framework.urlpatterns import format_suffix_patterns
from rest_framework_jwt.views import obtain_jwt_token, verify_jwt_token, refresh_jwt_token

from financeApi.views import users, payments, balances

urlpatterns = [
    url(r'^api-token-auth/', obtain_jwt_token),
    url(r'^api-token-verify/', verify_jwt_token),
    url(r'^api-token-refresh/', refresh_jwt_token),
    url(r'^users/$', users.users_creation),
    url(r'^payments/$', payments.PaymentList.as_view()),
    url(r'^payments/(?P<pk>[0-9]+)/$', payments.PaymentDetail.as_view()),
    url(r'^balances/$', balances.BalanceList.as_view()),
    url(r'^balances/(?P<pk>[0-9]+)/$', balances.BalanceDetail.as_view()),
]

urlpatterns = format_suffix_patterns(urlpatterns)
