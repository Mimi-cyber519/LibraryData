namespace LibraryData.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        // Внешние ключи
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }
    }
}
