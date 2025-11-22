using Microsoft.EntityFrameworkCore;
using UzStay.Api.Models.Foundations.Guests;

namespace UzStay.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Guest> Guests { get; set; }
    }
}
