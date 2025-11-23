using UzStay.Api.Models.Foundations.Guests;
using UzStay.Api.Models.Foundations.Guests.Exception;

namespace UzStay.Api.Services.Foundations.Guests
{
    public partial class GuestService
    {
        private void ValidateGuestNotNull(Guest guest)
        {
            if (guest is null)
                throw new NullGuestException();
        }
    }
}
