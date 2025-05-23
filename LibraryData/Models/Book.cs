﻿namespace LibraryData.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public ICollection<BookGenre> BookGenres { get; set; }
        public ICollection<BookPublisher> BookPublishers { get; set; }

        public ICollection<Loan> Loans { get; set; }
    }

}
