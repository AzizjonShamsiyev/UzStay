using Xeptions;

namespace UzStay.Api.Models.Foundations.Guests.Exceptions
{
    public class GuestValidationException : Xeption
    {
        public GuestValidationException(Xeption innerException)
            : base(message: "Guest validation error occurred, fix the errors and try again",
                 innerException)
        { }
    }
}

