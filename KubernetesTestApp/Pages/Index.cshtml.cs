using KubernetesTestApp.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KubernetesTestApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly AppDbContext _dbContext;

    [BindProperty]
    public string ProfileInfo { get; set; } = "no data";

    [BindProperty]
    public string EnvironmentName { get; set; } = "no data";

    [BindProperty]
    public string AspnetcoreEnvironment { get; set; } = "no data";

    public IndexModel(ILogger<IndexModel> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public void OnGet()
    {
        EnvironmentName = Environment.GetEnvironmentVariable("HOST_NAME") ?? "no data [HOST_NAME]";
        AspnetcoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "no data [ASPNETCORE_ENVIRONMENT]";
        
        ShowProfileInfo();
    }

    private void ShowProfileInfo()
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