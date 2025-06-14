﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryData.Models;

namespace LibraryData.Controllers
{
    public class LoansController : Controller
    {
        private readonly LibraryContext _context;

        public LoansController(LibraryContext context)
        {
            _context = context;
        }

  
        public async Task<IActionResult> Index(string bookTitle, string memberName)
        {
            var loans = _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .AsQueryable();

        
            if (!string.IsNullOrEmpty(bookTitle))
            {
                loans = loans.Where(l => l.Book.Title.Contains(bookTitle));
            }

            if (!string.IsNullOrEmpty(memberName))
            {
                loans = loans.Where(l => l.Member.Name.Contains(memberName));
            }

            
            loans = loans.OrderByDescending(l => l.Date);

            return View(await loans.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

     
        public IActionResult Create()
        {
            ViewBag.BookList = new SelectList(_context.Books, "Id", "Title");
            ViewBag.MemberList = new SelectList(_context.Members, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,BookId,MemberId")] Loan loan)
        {

                _context.Add(loan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", loan.BookId);
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Id", loan.MemberId);
            return View(loan);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", loan.BookId);
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Name", loan.MemberId);
            return View(loan);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,BookId,MemberId")] Loan loan)
        {
            if (id != loan.Id)
            {
                return NotFound();
            }


                try
                {
                    _context.Update(loan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(loan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", loan.BookId);
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Id", loan.MemberId);
            return View(loan);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan != null)
            {
                _context.Loans.Remove(loan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.Id == id);
        }
    }
}
