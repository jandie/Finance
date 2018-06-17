from rest_framework import permissions


class IsPaymentOwner(permissions.BasePermission):
    """
    Custom permission to only allow owners of a payment to get, edit or delete it.
    """
    def has_object_permission(self, request, view, obj):
        return obj.user == request.user


class IsBalanceOwner(permissions.BasePermission):
    """
    Custom permission to only allow owners of a payment to get, edit or delete it.
    """
    def has_object_permission(self, request, view, obj):
        print(obj.user)
        print(request.user)
        return obj.user == request.user