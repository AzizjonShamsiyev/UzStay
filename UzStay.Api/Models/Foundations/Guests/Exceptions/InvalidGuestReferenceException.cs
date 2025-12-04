using System;
using Xeptions;

namespace UzStay.Api.Models.Foundations.Guests.Exceptions
{
    public class InvalidGuestReferenceException : Xeption
    {
        public InvalidGuestReferenceException(Exception innerException)
			: base(message: "Invalid guest reference error occurred.", innerException)
		{ }
    }
}
