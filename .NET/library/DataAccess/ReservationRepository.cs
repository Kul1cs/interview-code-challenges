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
				var book = context.Books.FirstOrDefault(x => x.Id == bookId);
				var borrower = context.Borrowers.FirstOrDefault(x => x.Id == borrowerId);
				// Could assume we can only place reservations when the book is loaned
				var lastReservationEndTime = GetNextAvailability(bookId);
				if (book == null || borrower == null)
				{
					throw new Exception("Invalid data provided");
				}
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
