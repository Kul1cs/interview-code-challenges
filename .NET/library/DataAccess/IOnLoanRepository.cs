using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface IOnLoanRepository
    {
        public List<OnLoan> GetLoans();
        public void AddFine(Fine fine);
        public void ReturnLoan(BookStock bookStock);
    }
}
