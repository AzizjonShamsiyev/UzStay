using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading.Tasks;
using UzStay.Api.Models.Foundations.Guests;

namespace UzStay.Api.Brokers.Storages
{
    public partial class StorageBroker : IStorageBroker
    {
        public DbSet<Guest> Guests { get; set; }

        public async ValueTask<Guest> InsertGuestAsync(Guest guests)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Guest> guestEntityEntry = await broker.Guests.AddAsync(guests);
            await broker.SaveChangesAsync();
            return guestEntityEntry.Entity;
        }

        public IQueryable<Guest> SelectAllGuests() => 
            this.Guests.AsNoTracking();

        public ValueTask<Guest> SelectGuestByIdAsync(Guid guestId) =>
            this.Guests.FindAsync(guestId);

        public async ValueTask<Guest> UpdateGuestAsync(Guest guest)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Guest> guestEntityEntry = broker.Guests.Update(guest);
            await broker.SaveChangesAsync();

            return guestEntityEntry.Entity;
        }

        public async ValueTask<Guest> DeleteGuestAsync(Guest guest)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Guest> guestEntityEntry = broker.Guests.Remove(guest);
            await broker.SaveChangesAsync();

            return guestEntityEntry.Entity;
        }
    }
}