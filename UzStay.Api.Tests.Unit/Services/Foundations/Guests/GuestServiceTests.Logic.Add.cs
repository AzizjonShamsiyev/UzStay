using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Tynamix.ObjectFiller;
using UzStay.Api.Brokers.Storages;
using UzStay.Api.Models.Foundations.Guests;
using UzStay.Api.Services.Foundations.Guests;
using Xunit;

namespace UzStay.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldAddAsync()
        {
            //given
            Guest randomGuest = CreateRandomGuest();
            Guest inputGuest = randomGuest;
            Guest storageGuest = inputGuest;
            Guest expectedGuest = storageGuest.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuestsAsync(inputGuest))
                .ReturnsAsync(storageGuest);

            //when
            Guest actualGuest = await this.guestSevice.AddGuestAsync(inputGuest);

            //then
            actualGuest.Should().BeEquivalentTo(expectedGuest);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuestsAsync(inputGuest), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
