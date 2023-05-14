namespace ResourceManager.Application.Common;

public class ConcurrencyException : ApplicationLogicException
{
    public ConcurrencyException(Exception e) : base("The record has been modified concurrently", e)
    {
    }
}
