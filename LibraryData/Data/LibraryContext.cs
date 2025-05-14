using LibraryData.Models;
using Microsoft.EntityFrameworkCore;

public class LibraryContext : DbContext
{
   

    // DbSet для таблиц
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Publisher> Publishers => Set<Publisher>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Loan> Loans => Set<Loan>();

    // Связующие таблицы (многие-ко-многим)
    public DbSet<BookGenre> BookGenres => Set<BookGenre>();
    public DbSet<BookPublisher> BookPublishers => Set<BookPublisher>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Настройка связующих таблиц (многие-ко-многим)
        modelBuilder.Entity<BookGenre>()
            .HasKey(bg => new { bg.GenresId, bg.BooksId });
        modelBuilder.Entity<BookGenre>()
            .HasOne(bg => bg.Book)
            .WithMany(b => b.BookGenres)
            .HasForeignKey(bg => bg.BooksId);
        modelBuilder.Entity<BookGenre>()
            .HasOne(bg => bg.Genre)
            .WithMany(g => g.BookGenres)
            .HasForeignKey(bg => bg.GenresId);
        modelBuilder.Entity<BookPublisher>()
            .HasKey(bp => new { bp.BooksId, bp.PublishersId });
        modelBuilder.Entity<BookPublisher>()
            .HasOne(bp => bp.Book)
            .WithMany(b => b.BookPublishers)
            .HasForeignKey(bp => bp.BooksId);
        modelBuilder.Entity<BookPublisher>()
            .HasOne(bp => bp.Publisher)
            .WithMany(p => p.BookPublishers)
            .HasForeignKey(bp => bp.PublishersId);
    }
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
    {
        Database.Migrate();
    }
}