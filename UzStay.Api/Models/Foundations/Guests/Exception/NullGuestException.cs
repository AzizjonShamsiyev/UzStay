using Xeptions;

namespace UzStay.Api.Models.Foundations.Guests.Exception
{
    public class NullGuestException : Xeption
    {
        public NullGuestException() 
            :base(message: "Guest is null")
        { }
    }
}
