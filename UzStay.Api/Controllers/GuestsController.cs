using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using System.Threading.Tasks;
using UzStay.Api.Models.Foundations.Guests;
using UzStay.Api.Models.Foundations.Guests.Exceptions;
using UzStay.Api.Services.Foundations.Guests;

namespace UzStay.Api.Controllers
{
    [ApiController]
    [Route("api / [controller]")]
    public class GuestsController : RESTFulController
    {
        private readonly IGuestService guestService;

        public GuestsController(IGuestService guestService)
        {
            this.guestService = guestService;
        }

        [HttpPost]
        public async ValueTask<ActionResult<Guest>> PostGuestAsync(Guest guest)
        {
            try
            {
                Guest postedGuest = 
                    await this.guestService.AddGuestAsync(guest);
                return Created(postedGuest);
            }
            catch(GuestValidationException guestValidationException)
            {
                return BadRequest(guestValidationException.InnerException);
            }
		    catch (GuestDependencyValidationException guestDependencyValidationException)
			    when(guestDependencyValidationException.InnerException is AlreadyExistGuestException)
            {
                return Conflict(guestDependencyValidationException.InnerException);
            }
            catch (GuestDependencyValidationException guestDependencyValidationException)
            {
                return BadRequest(guestDependencyValidationException.InnerException);
            }
            catch(GuestDependencyException guestDependencyException)
            {
                return InternalServerError(guestDependencyException.InnerException);
            }
            catch(GuestServiceException guestServiceException)
            {
                return InternalServerError(guestServiceException.InnerException);
            }
        }
    }
}
