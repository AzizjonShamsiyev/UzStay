using FluentAssertions;
using Moq;
using UzStay.Api.Models.Foundations.Guests;
using UzStay.Api.Models.Foundations.Guests.Exceptions;
using Xunit;

namespace UzStay.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfGuestIsNullAndLogItAsync()
        {
            //given
            Guest nullGuest = null;
            var nullGuestException = new NullGuestException();

            var expectedGuestValidationException =
                new GuestValidationException(nullGuestException);

            //when
            ValueTask<Guest> addGuestTask =
                this.guestService.AddGuestAsync(nullGuest);

            GuestValidationException actualGuestValidationException =
                await Assert.ThrowsAsync<GuestValidationException>(
                    addGuestTask.AsTask);

            //then
            actualGuestValidationException.Should()
                .BeEquivalentTo(expectedGuestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuestAsync(nullGuest),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfGuestIsInvalidAndLogItAsync(
            string invalidText)
        {
            //given
            var invalidGuest = new Guest
            {
                FirstName = invalidText,
                Email = invalidText
            };

            var invalidGuestException = new InvalidGuestException();

            invalidGuestException.AddData(
                key: nameof(Guest.Id),
                values: $"{nameof(Guest.Id)} is required");

            invalidGuestException.AddData(
                key: nameof(Guest.FirstName),
                values: $"{nameof(Guest.FirstName)} is required");

            invalidGuestException.AddData(
                key: nameof(Guest.LastName),
                values: $"{nameof(Guest.LastName)} is required");

            invalidGuestException.AddData(
                key: nameof(Guest.DateOfBirth),
                values: $"{nameof(Guest.DateOfBirth)} is required");

            invalidGuestException.AddData(
                key: nameof(Guest.Email),
                values: $"{nameof(Guest.Email)} is required");

            invalidGuestException.AddData(
                key: nameof(Guest.Address),
                values: $"{nameof(Guest.Address)} is required");

            var expectedGuestValidationException =
                new GuestValidationException(invalidGuestException);

            //when
            ValueTask<Guest> addGuestTask =
                this.guestService.AddGuestAsync(invalidGuest);

            GuestValidationException actualGuestValidationException =
                await Assert.ThrowsAsync<GuestValidationException>(
                    addGuestTask.AsTask);

            //then
            actualGuestValidationException.Should()
                .BeEquivalentTo(expectedGuestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuestAsync(invalidGuest),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfGenderIsInvalidAndLogItAsync()
        {
            //given
            Guest randomGuest = CreateRandomGuest();
            Guest invalidGuest = randomGuest;
            invalidGuest.Gender = GetInvalidEnum<GenderType>();
            var invalidGuestException = new InvalidGuestException();
            invalidGuestException.AddData(
                key: nameof(Guest.Gender),
                values: $"{nameof(Guest.Gender)} is required");

            var expectedGuestValidationException =
                new GuestValidationException(invalidGuestException);

            //when
            ValueTask<Guest> addGuestTask =
                this.guestService.AddGuestAsync(invalidGuest);

            GuestValidationException actualGuestValidationException =
                await Assert.ThrowsAsync<GuestValidationException>(
                    addGuestTask.AsTask);

            //then
            actualGuestValidationException.Should()
                .BeEquivalentTo(expectedGuestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuestAsync(It.IsAny<Guest>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDateIsNotSameAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            int randomNumber = GetRandomNumber();
            Guest randomGuest = CreateRandomGuest(randomDateTime);
            Guest invalidGuest = randomGuest;

            invalidGuest.UpdatedDate =
                invalidGuest.CreatedDate.AddDays(randomNumber);

            var invalidGuestException =
                new InvalidGuestException();

            invalidGuestException.AddData(
                key: nameof(Guest.UpdatedDate),
                values: $"Date is not the same as {nameof(Guest.CreatedDate)}");

            var expectedGuestValidationException =
                new GuestValidationException(invalidGuestException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);
            //when
            ValueTask<Guest> addGuestTask =
                this.guestService.AddGuestAsync(invalidGuest);

            GuestValidationException actualGuestValidationException =
                await Assert.ThrowsAsync<GuestValidationException>(
                    addGuestTask.AsTask);
            //then
            actualGuestValidationException.Should()
                .BeEquivalentTo(expectedGuestValidationException);
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);
            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuestAsync(It.IsAny<Guest>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
