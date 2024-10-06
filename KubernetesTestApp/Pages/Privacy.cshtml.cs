using KubernetesTestApp.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KubernetesTestApp.Pages;

public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;
    private readonly AppDbContext _dbContext;
    
    [BindProperty]
    public string ProfileInfo { get; set; } = "no data";

    public PrivacyModel(ILogger<PrivacyModel> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public void OnGet()
    {
        GetProfileInfo();
    }
    
    private void GetProfileInfo()
    {
        try
        {
            var profile = _dbContext.Profiles.ToList().FirstOrDefault();
            if (profile is null)
            {
                ProfileInfo = "no data in database";
            }
            else
            {
                ProfileInfo = $"[{profile.Id}] {profile.FirstName} {profile.LastName}: {profile.Profession}";    
            }

        }
        catch (Exception e)
        {
            ProfileInfo = $"[ERROR] {e}";
        }
    }
}