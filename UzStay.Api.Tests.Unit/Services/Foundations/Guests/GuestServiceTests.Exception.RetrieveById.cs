using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UzStay.Api.Models.Foundations.Guests.Exceptions;
using UzStay.Api.Models.Foundations.Guests;
using Xunit;
using FluentAssertions;

namespace UzStay.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid someGuestId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedGuestStorageException =
            new FailedGuestStorageException(sqlException);

            var expectedGuestDependencyException =
                new GuestDependencyException(failedGuestStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(someGuestId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Guest> retrieveGuestByIdTask =
                this.guestService.RetrieveGuestByIdAsync(someGuestId);

            GuestDependencyException actualGuestDependencyException =
                await Assert.ThrowsAsync<GuestDependencyException>(
                    retrieveGuestByIdTask.AsTask);

            // then
            actualGuestDependencyException.Should().BeEquivalentTo(
                expectedGuestDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGuestDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(someGuestId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
