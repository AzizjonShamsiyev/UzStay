using System.Threading.Tasks;
using UzStay.Api.Brokers.Logging;
using UzStay.Api.Brokers.Storages;
using UzStay.Api.Models.Foundations.Guests;

namespace UzStay.Api.Services.Foundations.Guests
{
    public class GuestService : IGuestService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public GuestService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<Guest> AddGuestAsync(Guest guest) =>
            await this.storageBroker.InsertGuestsAsync(guest);
    }
}
