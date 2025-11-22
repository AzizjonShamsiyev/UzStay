using System.Threading.Tasks;
using UzStay.Api.Brokers.Storages;
using UzStay.Api.Models.Foundations.Guests;

namespace UzStay.Api.Services.Foundations.Guests
{
    public class GuestService : IGuestService
    {
        private readonly IStorageBroker storageBroker;

        public GuestService(IStorageBroker storageBroker) =>
            this.storageBroker = storageBroker;

        public async ValueTask<Guest> AddGuestAsync(Guest guest) =>
            await this.storageBroker.InsertGuestsAsync(guest);
    }
}
