using Microsoft.Data.SqlClient;
using Moq;
using UzStay.Api.Models.Foundations.Guests.Exceptions;
using UzStay.Api.Models.Foundations.Guests;
using Xunit;

namespace UzStay.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guest someGuest = CreateRandomGuest();
            SqlException sqlException = GetSqlError();

            var failedGuestStorageException = 
                new FailedGuestStorageException(sqlException);

            var expectedGuestDependencyException =
                new GuestDependencyException(failedGuestStorageException);

            this.storageBrokerMock.Setup(broker =>
             broker.InsertGuestAsync(someGuest))
              .ThrowsAsync(sqlException);

            //when
            ValueTask<Guest> addGuestTask =
                this.guestService.AddGuestAsync(someGuest);
            //then
            await Assert.ThrowsAsync<GuestDependencyException>(() =>
                addGuestTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuestAsync(someGuest),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGuestDependencyException))),
                     Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
