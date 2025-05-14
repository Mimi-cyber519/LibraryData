namespace LibraryData.Models
{
    public class BookPublisher
    {
        public int BooksId { get; set; }
        public Book Book { get; set; }

        public int PublishersId { get; set; }
        public Publisher Publisher { get; set; }
    }
}
