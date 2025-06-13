using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface IReservationRepository
    {
        public DateTime GetNextAvailability(Guid bookId);

        public void PlaceReservation(Guid borrowerId, Guid bookId, int days);
    }
}
