from django.contrib import admin
from financeApi.models import Balance, Payment, Transaction, BalanceHistory

admin.site.register(Balance)
admin.site.register(Payment)
admin.site.register(Transaction)
admin.site.register(BalanceHistory)