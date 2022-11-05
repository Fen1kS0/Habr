namespace Habr.Common.Exceptions;

public class BaseException : Exception
{
    public BaseException(string message) : base(message)
    {
        StatusCode = 500;
    }
    
    public BaseException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
    
    public int StatusCode { get; set; }
}