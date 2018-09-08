from django.contrib import admin
from financeApi.models import Balance, Payment, Transaction

admin.site.register(Balance)
admin.site.register(Payment)
admin.site.register(Transaction)
