namespace Habr.Common.DTOs.V1.Errors;

public class ServerErrorResponse
{
    public ServerErrorResponse(int statusCode, string errorMessage, string? stackTrace)
    {
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
        StackTrace = stackTrace;
    }

    public int StatusCode { get; set; }
    public string ErrorMessage { get; set; }
    public string? StackTrace { get; set; }
}