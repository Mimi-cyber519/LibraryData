using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryData.Models;

namespace LibraryData.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Books
        // Controller (BooksController.cs)
        public async Task<IActionResult> Index(string searchString)
        {
            var books = _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .Include(b => b.BookPublishers)
                    .ThenInclude(bp => bp.Publisher)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b =>
                    b.Title.Contains(searchString) ||
                    (b.Author != null && b.Author.Name.Contains(searchString)) ||
                    b.BookGenres.Any(bg => bg.Genre.Name.Contains(searchString)) ||
                    b.BookPublishers.Any(bp => bp.Publisher.Name.Contains(searchString)));
            }

            return View(await books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var book = await _context.Books
        .Include(b => b.Author)
        .Include(b => b.BookGenres)
            .ThenInclude(bg => bg.Genre)
        .Include(b => b.BookPublishers)
            .ThenInclude(bp => bp.Publisher)
        .FirstOrDefaultAsync(m => m.Id == id);
            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewBag.AuthorList = new SelectList(_context.Authors, "Id", "Name");
            ViewBag.GenreList = new MultiSelectList(_context.Genres, "Id", "Name");
            ViewBag.PublisherList = new MultiSelectList(_context.Publishers, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, int[] SelectedGenres, int[] SelectedPublishers)
        {

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            foreach (var genreId in SelectedGenres)
            {
                _context.BookGenres.Add(new BookGenre { BooksId = book.Id, GenresId = genreId });
            }

            foreach (var publisherId in SelectedPublishers)
            {
                _context.BookPublishers.Add(new BookPublisher { BooksId = book.Id, PublishersId = publisherId });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
{
            var book = await _context.Books
                .Include(b => b.BookGenres)
                .Include(b => b.BookPublishers)
                .FirstOrDefaultAsync(b => b.Id == id);

            ViewBag.AuthorList = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);
            ViewBag.GenreList = new MultiSelectList(_context.Genres, "Id", "Name", book.BookGenres.Select(bg => bg.GenresId));
            ViewBag.PublisherList = new MultiSelectList(_context.Publishers, "Id", "Name", book.BookPublishers.Select(bp => bp.PublishersId));
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book book, int[] SelectedGenres, int[] SelectedPublishers)
        {
            _context.Update(book);

            // Update genres
            var currentGenres = await _context.BookGenres.Where(bg => bg.BooksId == book.Id).ToListAsync();
            _context.BookGenres.RemoveRange(currentGenres);
            foreach (var genreId in SelectedGenres)
            {
                _context.BookGenres.Add(new BookGenre { BooksId = book.Id, GenresId = genreId });
            }

            // Update publishers
            var currentPublishers = await _context.BookPublishers.Where(bp => bp.BooksId == book.Id).ToListAsync();
            _context.BookPublishers.RemoveRange(currentPublishers);
            foreach (var publisherId in SelectedPublishers)
            {
                _context.BookPublishers.Add(new BookPublisher { BooksId = book.Id, PublishersId = publisherId });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .Include(b => b.BookPublishers)
                    .ThenInclude(bp => bp.Publisher)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books
        .Include(b => b.BookGenres)
        .Include(b => b.BookPublishers)
        .FirstOrDefaultAsync(b => b.Id == id);

            if (book != null)
            {
                // Удаляем связанные записи
                _context.BookGenres.RemoveRange(book.BookGenres);
                _context.BookPublishers.RemoveRange(book.BookPublishers);
                _context.Books.Remove(book);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
