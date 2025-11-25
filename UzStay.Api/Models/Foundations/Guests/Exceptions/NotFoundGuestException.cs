using System;
using Xeptions;

namespace UzStay.Api.Models.Foundations.Guests.Exceptions
{
    public class NotFoundGuestException : Xeption
    {
        public NotFoundGuestException(Guid guestId)
            : base(message: $"Couldn't find guest with id: {guestId}")
        { }
    }
}
