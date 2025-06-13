using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
	public class OnLoanRepository : IOnLoanRepository
	{
		public OnLoanRepository()
		{
		}

		public List<OnLoan> GetLoans()
		{
			using (var context = new LibraryContext())
			{
				return context.Catalogue
					.Where(x => x.OnLoanTo != null)
					.Include(x => x.Book)
					.Include(x => x.OnLoanTo)
					.Select(x => new OnLoan() { BookName = x.Book.Name, Borrower = x.OnLoanTo }) // Could simply ommit the whole OnLoan part and return BookStock with all the book data
					.ToList();
			}
		}

		public Fine ReturnLoan(Guid bookStockId)
		{
			using (var context = new LibraryContext())
			{
				var bookStock = context.Catalogue.First(x => x.Id == bookStockId);
				var currentTime = DateTime.UtcNow;
				Fine fine = null;
				if (bookStock.LoanEndDate.HasValue && bookStock.LoanEndDate < currentTime)
				{
					fine = new Fine
					{
						Amount = (currentTime - bookStock.LoanEndDate).Value.Days * 10,
						Book = bookStock.Book,
						Borrower = bookStock.OnLoanTo,
						LoanEndDate = bookStock.LoanEndDate.Value,
						ReturnedOnDate = currentTime
					};
					context.Fines.Add(fine);
					var nextReservation = context.Reservations.OrderBy(x => x.ReservationStartTime).FirstOrDefault();
					if (nextReservation != null)
					{
						bookStock.LoanEndDate = nextReservation.ReservationEndTime;
						bookStock.OnLoanTo = nextReservation.Borrower;
						context.Reservations.Remove(nextReservation);
					}
					else
					{
						bookStock.LoanEndDate = null;
						bookStock.OnLoanTo = null;
					}
					context.SaveChanges();
				}
				return fine;
			}
		}
	}
}
