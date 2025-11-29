using MAlex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MAlex.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }


        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(ContactMessage model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill all required fields correctly.";
                return View("Contact", model);
            }

            model.SentAt = System.DateTime.Now;
            _context.ContactMessages.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your message has been sent successfully!";
            return RedirectToAction("Contact");
        } 


        public async Task<IActionResult> ContactAdmin()
        {

            var messages = await _context.ContactMessages
                                         .OrderByDescending(m => m.SentAt)
                                         .ToListAsync();

            return View(messages);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var contactMessage = await _context.ContactMessages.FindAsync(id);

            if (contactMessage == null)
            {
                TempData["ErrorMessage"] = "Message not found.";
                return RedirectToAction(nameof(ContactAdmin));
            }

            _context.ContactMessages.Remove(contactMessage);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Message deleted successfully.";
            return RedirectToAction(nameof(ContactAdmin));
        }
    }
}