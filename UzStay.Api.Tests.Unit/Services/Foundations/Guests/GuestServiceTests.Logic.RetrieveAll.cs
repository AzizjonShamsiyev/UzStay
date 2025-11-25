using FluentAssertions;
using Moq;
using UzStay.Api.Models.Foundations.Guests;
using Xunit;

namespace UzStay.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllGuests()
        {
            //given
            IQueryable<Guest> randomGuests = CreateRandomGuests();
            IQueryable<Guest> storageGuests = randomGuests;
            IQueryable<Guest> expectedGuests = storageGuests;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuests())
                    .Returns(storageGuests);

            //when
            IQueryable<Guest> actualGuests = 
                this.guestService.RetrieveAllGuests();

            //then
            actualGuests.Should().BeEquivalentTo(expectedGuests);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuests(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
