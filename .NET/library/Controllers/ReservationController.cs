using Microsoft.AspNetCore.Mvc;
using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;

namespace OneBeyondApi.Controllers
{
	[ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ILogger<ReservationController> _logger;
        private readonly IReservationRepository _ReservationRepository;
		private readonly ICatalogueRepository _CatalogueRepository;

		public ReservationController(ILogger<ReservationController> logger, IReservationRepository ReservationRepository, ICatalogueRepository CatalogueRepository)
        {
            _logger = logger;
            _ReservationRepository = ReservationRepository;
			_CatalogueRepository = CatalogueRepository;   
        }

        [HttpPost]
        [Route("NextAvailability")]
        public DateTime? Post(Guid bookId)
        {
            return _ReservationRepository.GetNextAvailability(bookId);
        }

        [HttpPost]
        [Route("PlaceReservation")]
        public void Post(Guid borrowerId, Guid bookId, int days)
        {
            _ReservationRepository.PlaceReservation(borrowerId, bookId, days);
		}
    }
}