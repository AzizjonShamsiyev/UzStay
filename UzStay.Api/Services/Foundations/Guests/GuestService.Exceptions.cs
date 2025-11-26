using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;
using UzStay.Api.Models.Foundations.Guests;
using UzStay.Api.Models.Foundations.Guests.Exceptions;
using Xeptions;

namespace UzStay.Api.Services.Foundations.Guests
{
    public partial class GuestService
    {
        private delegate IQueryable<Guest> ReturningGuestsFunction();
        private delegate ValueTask<Guest> ReturningGuestFunction();

        private async ValueTask<Guest> TryCatch(ReturningGuestFunction returningGuestFunction)
        {
            try
            {
                return await returningGuestFunction();
            }
            catch (NullGuestException nullGuestException)
            {
                throw CreateAndLogValidationException(nullGuestException);
            }
            catch (InvalidGuestException invalidGuestException)
            {
                throw CreateAndLogValidationException(invalidGuestException);
            }
            catch (SqlException sqlException)
            {
                var failedGuestStorageException =
                    new FailedGuestStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedGuestStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistGuestException =
                    new AlreadyExistGuestException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistGuestException);
            }
            catch(NotFoundGuestException notFoundGuestException)
            {
                throw CreateAndLogValidationException(notFoundGuestException);
            }
            catch (Exception exception)
            {
                var failedGuestServiceException =
                    new FailedGuestServiceException(exception);

                throw CreateAndLogServiceException(failedGuestServiceException);
            }
        }

        private IQueryable<Guest> TryCatch(ReturningGuestsFunction returningGuestsFunction)
        {
            try
            {
                return returningGuestsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedGuestStorageException =
                    new FailedGuestStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedGuestStorageException);
            }
        }

        private GuestDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var guestDependencyException =
                new GuestDependencyException(exception);

            this.loggingBroker.LogCritical(guestDependencyException);

            return guestDependencyException;
        }

        private GuestDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var guestDependencyValidationException =
               new GuestDependencyValidationException(exception);

            this.loggingBroker.LogError(guestDependencyValidationException);

            return guestDependencyValidationException;
        }

        private GuestValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var guestValidationException =
                new GuestValidationException(exception);

            this.loggingBroker.LogError(guestValidationException);

            return guestValidationException;
        }

        private GuestServiceException CreateAndLogServiceException(Xeption exception)
        {
            var guestServiceException = new GuestServiceException(exception);
            this.loggingBroker.LogError(guestServiceException);

            return guestServiceException;
        }
    }
}
