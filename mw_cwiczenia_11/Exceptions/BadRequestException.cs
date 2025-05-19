namespace mw_cwiczenia_11.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(){}
    public BadRequestException(string message) : base(message){}
}