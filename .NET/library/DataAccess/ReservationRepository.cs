using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
	public class ReservationRepository : IReservationRepository
	{
		public ReservationRepository()
		{
		}

		public DateTime GetNextAvailability(Guid bookId)
		{
			using (var context = new LibraryContext())
			{
				return context.Reservations.Where(x => x.Book.Id == bookId)?.Max(x => x.ReservationEndTime) ?? DateTime.UtcNow;
			}
		}

		public void PlaceReservation(Guid borrowerId, Guid bookId, int days)
		{
			using (var context = new LibraryContext())
			{
				// Will assume we can only place reservations when the book is loaned as per the requirements
				var book = context.Books.First(x => x.Id == bookId);
				var borrower = context.Borrowers.First(x => x.Id == borrowerId);
				var lastReservationEndTime = GetNextAvailability(bookId);
				context.Reservations.Add(new Reservation
				{
					Book = book,
					Borrower = borrower,
					ReservationStartTime = lastReservationEndTime,
					ReservationEndTime = lastReservationEndTime.AddDays(days),
				}
				);
			}
		}
	}
}
