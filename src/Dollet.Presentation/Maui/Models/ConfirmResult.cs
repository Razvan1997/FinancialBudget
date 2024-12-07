namespace Dollet.Models
{
    public record ConfirmResult(bool ConfirmedForce, bool Confirmed)
    {
        bool IsConfirmedForce() => ConfirmedForce;
        bool IsConfirmed() => Confirmed;
    }
}