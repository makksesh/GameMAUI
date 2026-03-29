namespace MobileApp.Exceptions;

public class AccountBlockedException : Exception
{
    public AccountBlockedException()
        : base("Ваш аккаунт заблокирован.") { }
}