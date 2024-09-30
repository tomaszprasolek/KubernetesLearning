using KubernetesTestApp.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KubernetesTestApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly AppDbContext _dbContext;

    [BindProperty]
    public string ProfileInfo { get; set; }

    public IndexModel(ILogger<IndexModel> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public void OnGet()
    {
        var profile = _dbContext.Profiles.ToList().First();
        ProfileInfo = $"[{profile.Id}] {profile.FirstName} {profile.LastName}: {profile.Profession}";
    }
}