using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface IOnLoanRepository
    {
        public List<OnLoan> GetLoans();

        public Fine ReturnLoan(Guid bookStockId);
    }
}
