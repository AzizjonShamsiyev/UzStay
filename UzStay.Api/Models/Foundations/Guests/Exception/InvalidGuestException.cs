using Xeptions;

namespace UzStay.Api.Models.Foundations.Guests.Exception
{
    public class InvalidGuestException : Xeption
    {
        public InvalidGuestException()
            :base(message:"Guest is invalid")
        { }
    }
}
