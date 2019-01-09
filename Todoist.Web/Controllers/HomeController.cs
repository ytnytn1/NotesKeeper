using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotesKeeper.Data;
using NotesKeeper.Models;
using NotesKeeper.ViewModels;

namespace NotesKeeper.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var author = await _userManager.FindByNameAsync(User.Identity.Name);
            var applicationDbContext = _context.Notes.
                Where(n => n.AuthorId == author.Id)
                .Select(n => new NoteGetVm {Id = n.Id, Body = n.Body, Title = n.Title, DateCreationUTC = n.DateCreationUTC});
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .Include(n => n.Author)
                .Select(n => new NoteGetVm
                {
                    Body = n.Body,
                    Id = n.Id,
                    Title = n.Title,
                    DateCreationUTC = n.DateCreationUTC
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Body")] AddEditNoteVm note)
        {
            if (ModelState.IsValid)
            {
                var author = await _userManager.FindByNameAsync(User.Identity.Name);
                var dbNote = new Note
                {
                    Id = Guid.NewGuid(),
                    AuthorId = author.Id,
                    Body = note.Body,
                    Title = note.Title
                };
               
                _context.Add(dbNote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            return View(note);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,Body")] AddEditNoteVm note)
        {
            

            if (ModelState.IsValid)
            {
                var dbNote = await _context.Notes.FindAsync(id);
                if (note == null)
                {
                    return NotFound();
                }
                dbNote.Body = note.Body;
                dbNote.Title = note.Title;
                try
                {
                    _context.Update(dbNote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(note);
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .Include(n => n.Author)
                .Select(n => new NoteGetVm
                {
                    DateCreationUTC = n.DateCreationUTC,
                    Body = n.Body,
                    Title = n.Title,
                    Id = n.Id
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var note = await _context.Notes.FindAsync(id);
            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(Guid id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
    }
}
