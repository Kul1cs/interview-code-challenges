using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface ICatalogueRepository
    {
        public List<BookStock> GetCatalogue();
        public BookStock GetBookStockByBookAndBorrower(Guid bookId, Guid borrowerId);

		public List<BookStock> SearchCatalogue(CatalogueSearch search);
    }
}
