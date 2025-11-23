using System;
using Xeptions;

namespace UzStay.Api.Models.Foundations.Guests.Exceptions
{
    public class FailedGuestStorageException : Xeption
    {
        public FailedGuestStorageException(Exception innerException)
            : base(message: "Failed guest storage error occurred, contact support", 
                  innerException)
        { }
    }
}
