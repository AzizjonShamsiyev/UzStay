using System;
using Xeptions;

namespace UzStay.Api.Models.Foundations.Guests.Exceptions
{
    public class FailedGuestServiceException : Xeption
    {
        public FailedGuestServiceException(Exception innerException)
            : base(message: "Failed guest service error occurred, contact support",
                 innerException)
        { }
    }
}
