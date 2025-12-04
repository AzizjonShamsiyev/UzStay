using System;
using System.Linq;
using System.Threading.Tasks;
using UzStay.Api.Brokers.DateTimes;
using UzStay.Api.Brokers.Logging;
using UzStay.Api.Brokers.Storages;
using UzStay.Api.Models.Foundations.Guests;

namespace UzStay.Api.Services.Foundations.Guests
{
    public partial class GuestService : IGuestService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public GuestService(
            IStorageBroker storageBroker, 
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Guest> AddGuestAsync(Guest guest) =>
        TryCatch(async () =>
        {
            ValidateGuestOnAdd(guest);

            return await this.storageBroker.InsertGuestAsync(guest);
        });

        public ValueTask<Guest> ModifyGuestAsync(Guest guest) =>
        TryCatch(async () =>
        {
            ValidateGuestOnModify(guest);

            Guest maybeGuest =
                await this.storageBroker.SelectGuestByIdAsync(guest.Id);

            ValidateStorageGuest(maybeGuest, guest.Id);

            return await this.storageBroker.UpdateGuestAsync(guest);
        });

        public IQueryable<Guest> RetrieveAllGuests() =>
        TryCatch(() => this.storageBroker.SelectAllGuests());

        public ValueTask<Guest> RetrieveGuestByIdAsync(Guid guestId) =>
        TryCatch(async () =>
        {
            ValidateGuestId(guestId);

            Guest maybeGuest = await this.storageBroker
                .SelectGuestByIdAsync(guestId);

            ValidateStorageGuest(maybeGuest, guestId);

            return maybeGuest;
        });
    }
}
