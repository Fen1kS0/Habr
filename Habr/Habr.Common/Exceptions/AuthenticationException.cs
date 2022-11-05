namespace Habr.Common.Exceptions;

public class AuthenticationException : BaseException
{
    public AuthenticationException(string message) : base(message, 401)
    {
    }
}