using System;
using System.Linq;
using System.Threading.Tasks;
using UzStay.Api.Models.Foundations.Guests;

namespace UzStay.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Guest> InsertGuestAsync(Guest guests);
        IQueryable<Guest> SelectAllGuests();
        ValueTask<Guest> SelectGuestByIdAsync(Guid guestId);
        ValueTask<Guest> UpdateGuestAsync(Guest guest);
        ValueTask<Guest> DeleteGuestAsync(Guest guest);
    }
}
