namespace Habr.Common.Exceptions;

public class AccessDeniedException : BaseException
{
    public AccessDeniedException(string message) : base(message, 403)
    {
    }
}