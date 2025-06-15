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

		public void AddFine(Fine fine)
		{
			using (var context = new LibraryContext())
			{
				context.Fines.Add(fine);
				context.SaveChanges();
			}
		}

		public void ReturnLoan(BookStock bookStock)
		{
			using (var context = new LibraryContext())
			{
				context.Attach(bookStock);
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
		}
	}
}
