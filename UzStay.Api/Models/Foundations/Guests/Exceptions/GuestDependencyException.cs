using Xeptions;

namespace UzStay.Api.Models.Foundations.Guests.Exceptions
{
    public class GuestDependencyException : Xeption
    {
        public GuestDependencyException(Xeption innerException)
            :base(message: "Guest deprndency error occurred, contact support",
                 innerException)
        { }
    }
}
