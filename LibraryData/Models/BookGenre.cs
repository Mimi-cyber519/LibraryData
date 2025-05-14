namespace LibraryData.Models
{
    public class BookGenre
    {
        public int BooksId { get; set; }
        public Book Book { get; set; }

        public int GenresId { get; set; }
        public Genre Genre { get; set; }
    }
}
