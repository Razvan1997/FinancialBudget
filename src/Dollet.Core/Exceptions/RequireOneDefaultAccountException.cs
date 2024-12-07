namespace Dollet.Core.Exceptions
{
    public class RequireOneDefaultAccountException() : Exception("At least one account must be default."), IDolletDomainException { }
}
