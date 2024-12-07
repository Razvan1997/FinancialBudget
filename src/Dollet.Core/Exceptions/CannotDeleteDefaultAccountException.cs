namespace Dollet.Core.Exceptions
{
    public class CannotDeleteDefaultAccountException() : Exception("Cannot delete default account."), IDolletDomainException { }
}
