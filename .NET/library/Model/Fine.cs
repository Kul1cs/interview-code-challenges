namespace OneBeyondApi.Model
{
    public class Fine
    {
        public Guid Id { get; set; }
        public Borrower Borrower { get; set; }
        public Book Book { get; set; }
		public DateTime LoanEndDate { get; set; }
        public DateTime ReturnedOnDate { get; set; }
		public decimal Amount { get; set; }
	}
}