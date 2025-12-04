using System;
using UzStay.Api.Models.Foundations.Guests;
using UzStay.Api.Models.Foundations.Guests.Exceptions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UzStay.Api.Services.Foundations.Guests
{
    public partial class GuestService
    {
        private void ValidateGuestOnAdd(Guest guest)
        {
            ValidateGuestIsNotNull(guest);

            Validate(
                (Rule: IsInvalid(guest.Id, nameof(Guest.Id)), Parameter: nameof(Guest.Id)),
                (Rule: IsInvalid(guest.FirstName, nameof(Guest.FirstName)), Parameter: nameof(Guest.FirstName)),
                (Rule: IsInvalid(guest.LastName, nameof(Guest.LastName)), Parameter: nameof(Guest.LastName)),
                (Rule: IsInvalid(guest.DateOfBirth, nameof(Guest.DateOfBirth)), Parameter: nameof(Guest.DateOfBirth)),
                (Rule: IsInvalid(guest.Email, nameof(Guest.Email)), Parameter: nameof(Guest.Email)),
                (Rule: IsInvalid(guest.Address, nameof(Guest.Address)), Parameter: nameof(Guest.Address)),
                (Rule: IsInvalid(guest.Gender, nameof(Guest.Gender)), Parameter: nameof(Guest.Gender)),

                (Rule: IsNotSame(
                    firstDate: guest.UpdatedDate,
                    secondDate: guest.CreatedDate,
                    secondDateName: nameof(Guest.CreatedDate)),
                Parameter: nameof(Guest.UpdatedDate)),

                (Rule: IsNotRecent(guest.CreatedDate), Parameter: nameof(Guest.CreatedDate))); 
        }

        private void ValidateGuestOnModify(Guest guest)
        {
            ValidateGuestIsNotNull(guest);

            Validate(
                (Rule: IsInvalid(guest.Id, nameof(Guest.Id)), Parameter: nameof(Guest.Id)),
                (Rule: IsInvalid(guest.FirstName, nameof(Guest.FirstName)), Parameter: nameof(Guest.FirstName)),
                (Rule: IsInvalid(guest.LastName, nameof(Guest.LastName)), Parameter: nameof(Guest.LastName)),
                (Rule: IsInvalid(guest.DateOfBirth, nameof(Guest.DateOfBirth)), Parameter: nameof(Guest.DateOfBirth)),
                (Rule: IsInvalid(guest.Email, nameof(Guest.Email)), Parameter: nameof(Guest.Email)),
                (Rule: IsInvalid(guest.Address, nameof(Guest.Address)), Parameter: nameof(Guest.Address)),
                (Rule: IsInvalid(guest.Gender, nameof(Guest.Gender)), Parameter: nameof(Guest.Gender)));
        }

        public void ValidateGuestId(Guid guestId) =>
           Validate((Rule: IsInvalid(guestId, nameof(Guest.Id)), Parameter: nameof(Guest.Id)));

        private void ValidateGuestIsNotNull(Guest guest)
        {
            if (guest is null)
                throw new NullGuestException();
        }

        private void ValidateStorageGuest(Guest maybeGuest, Guid guestId)
        {
            if (maybeGuest is null)
                throw new NotFoundGuestException(guestId);
        }

        private static dynamic IsInvalid(Guid Id, string parameterName) => new
        {
            Condition = Id == Guid.Empty,
            Message = $"{parameterName} is required"
        };

        private static dynamic IsInvalid(string text, string parameterName) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = $"{parameterName} is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date, string parameterName) => new
        {
            Condition = date == default,
            Message = $"{parameterName} is required"
        };

        private static dynamic IsInvalid(GenderType gender, string parameterName) => new
        {
            Condition = Enum.IsDefined(typeof(GenderType), gender) is false,
            Message = $"{parameterName} is required"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTimeOffset();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidGuestException = new InvalidGuestException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidGuestException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }
            invalidGuestException.ThrowIfContainsErrors();
        }
    }
}
