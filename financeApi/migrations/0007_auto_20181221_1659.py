# Generated by Django 2.1.4 on 2018-12-21 15:59

from django.db import migrations


class Migration(migrations.Migration):

    dependencies = [
        ('financeApi', '0006_transactionhistory'),
    ]

    operations = [
        migrations.RenameField(
            model_name='transactionhistory',
            old_name='transaction_idd',
            new_name='transaction_id',
        ),
    ]