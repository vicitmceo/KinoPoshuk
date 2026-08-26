namespace KinoPoshuk.SharedKernel.Exceptions;

public class ExternalApiException : Exception
{
    public ExternalApiException(string message) : base(message)
    {
    }
}
