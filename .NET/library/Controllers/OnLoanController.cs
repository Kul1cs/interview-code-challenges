using Microsoft.AspNetCore.Mvc;
using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;

namespace OneBeyondApi.Controllers
{
	[ApiController]
    [Route("[controller]")]
    public class OnLoanController : ControllerBase
    {
        private readonly ILogger<OnLoanController> _logger;
        private readonly IOnLoanRepository _LoanRepository;
		private readonly ICatalogueRepository _CatalogueRepository;

		public OnLoanController(ILogger<OnLoanController> logger, IOnLoanRepository LoanRepository, ICatalogueRepository CatalogueRepository)
        {
            _logger = logger;
            _LoanRepository = LoanRepository;
			_CatalogueRepository = CatalogueRepository;   
        }

        [HttpGet]
        [Route("GetLoans")]
        public IList<OnLoan> Get()
        {
            return _LoanRepository.GetLoans();
        }

        [HttpPost]
        [Route("ReturnLoan")]
        public Fine Post(Guid bookId, Guid borrowerId)
        {
			var bookStock = _CatalogueRepository.GetBookStockByBookAndBorrower(bookId, borrowerId);
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
                _LoanRepository.AddFine(fine);
            }
            _LoanRepository.ReturnLoan(bookStock);
			return fine;
        }
    }
}