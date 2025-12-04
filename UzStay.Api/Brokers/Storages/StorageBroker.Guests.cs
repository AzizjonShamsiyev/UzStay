using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading.Tasks;
using UzStay.Api.Models.Foundations.Guests;

namespace UzStay.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Guest> Guests { get; set; }

        public async ValueTask<Guest> InsertGuestAsync(Guest guest) =>
            await InsertAsync(guest);

        public IQueryable<Guest> SelectAllGuests() =>
            SelectAll<Guest>();

        public async ValueTask<Guest> SelectGuestByIdAsync(Guid guestId) =>
            await SelectAsync<Guest>(guestId);

        public async ValueTask<Guest> UpdateGuestAsync(Guest guest) =>
            await UpdateAsync(guest);

        public async ValueTask<Guest> DeleteGuestAsync(Guest guest) =>
            await DeleteAsync(guest);
    }
}