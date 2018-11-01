using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Miv.Data;
using Miv.Models;

namespace Miv.Controllers
{
    public class AttachingController : Controller
    {
        private readonly MivContext _context;

        public AttachingController(MivContext context)
        {
            _context = context;
        }

        // GET: Attaching
        public async Task<IActionResult> Index()
        {
            var mivContext = _context.Attachings.Include(a => a.Content).Include(a => a.Material);
            return View(await mivContext.ToListAsync());
        }

        // GET: Attaching/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attaching = await _context.Attachings
                .Include(a => a.Content)
                .Include(a => a.Material)
                .FirstOrDefaultAsync(m => m.AttachingID == id);
            if (attaching == null)
            {
                return NotFound();
            }

            return View(attaching);
        }

        // GET: Attaching/Create
        public IActionResult Create()
        {
            ViewData["ContentID"] = new SelectList(_context.Contents, "ContentID", "ContentID");
            ViewData["MaterialID"] = new SelectList(_context.Materials, "ID", "ID");
            return View();
        }

        // POST: Attaching/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttachingID,MaterialID,ContentID")] Attaching attaching)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attaching);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContentID"] = new SelectList(_context.Contents, "ContentID", "ContentID", attaching.ContentID);
            ViewData["MaterialID"] = new SelectList(_context.Materials, "ID", "ID", attaching.MaterialID);
            return View(attaching);
        }

        // GET: Attaching/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attaching = await _context.Attachings.FindAsync(id);
            if (attaching == null)
            {
                return NotFound();
            }
            ViewData["ContentID"] = new SelectList(_context.Contents, "ContentID", "ContentID", attaching.ContentID);
            ViewData["MaterialID"] = new SelectList(_context.Materials, "ID", "ID", attaching.MaterialID);
            return View(attaching);
        }

        // POST: Attaching/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AttachingID,MaterialID,ContentID")] Attaching attaching)
        {
            if (id != attaching.AttachingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attaching);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttachingExists(attaching.AttachingID))
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
            ViewData["ContentID"] = new SelectList(_context.Contents, "ContentID", "ContentID", attaching.ContentID);
            ViewData["MaterialID"] = new SelectList(_context.Materials, "ID", "ID", attaching.MaterialID);
            return View(attaching);
        }

        // GET: Attaching/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attaching = await _context.Attachings
                .Include(a => a.Content)
                .Include(a => a.Material)
                .FirstOrDefaultAsync(m => m.AttachingID == id);
            if (attaching == null)
            {
                return NotFound();
            }

            return View(attaching);
        }

        // POST: Attaching/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attaching = await _context.Attachings.FindAsync(id);
            _context.Attachings.Remove(attaching);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttachingExists(int id)
        {
            return _context.Attachings.Any(e => e.AttachingID == id);
        }
    }
}
