using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KubernetesTestApp.Database.Models;
using KubernetesTestApp.Database;

namespace KubernetesTestApp.Pages
{
    public class EditProfileModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditProfileModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Profile Profile { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var profileLocal = await _context.Profiles.FindAsync(id);

            if (profileLocal == null)
            {
                return NotFound();
            }

            Profile = profileLocal;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var profileToUpdate = await _context.Profiles.FindAsync(Profile.Id);

            if (profileToUpdate == null)
            {
                return NotFound();
            }

            profileToUpdate.FirstName = Profile.FirstName;
            profileToUpdate.LastName = Profile.LastName;
            profileToUpdate.Profession = Profile.Profession;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                if (!ProfileExists(Profile.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Refresh the data on the existing page
            Profile = profileToUpdate;
            return Page();
        }

        private bool ProfileExists(int id)
        {
            return _context.Profiles.Any(e => e.Id == id);
        }
    }
}