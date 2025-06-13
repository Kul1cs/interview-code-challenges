namespace OneBeyondApi.Model
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Book Book { get; set; }
        public Borrower Borrower { get; set; }
        public DateTime ReservationStartTime { get; set; }
        public DateTime ReservationEndTime { get; set; }
    }
}
