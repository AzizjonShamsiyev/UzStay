using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using UzStay.Api.Models.Foundations.Guests;
using UzStay.Api.Models.Foundations.Guests.Exceptions;
using Xunit;

namespace UzStay.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            //given
            SqlException sqlException = GetSqlException();

            var failedGuestStorageException =
                new FailedGuestStorageException(sqlException);

            var expectedGuestDependencyException =
                new GuestDependencyException(failedGuestStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuests())
                    .Throws(sqlException);

            //when
            Action retrieveAllGuestsAction = () =>
                this.guestService.RetrieveAllGuests();

            GuestDependencyException actualGuestDependencyException =
                Assert.Throws<GuestDependencyException>(
                    retrieveAllGuestsAction);

            //then
            actualGuestDependencyException.Should().BeEquivalentTo(
                expectedGuestDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuests(),
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
