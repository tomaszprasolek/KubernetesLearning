using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KubernetesTestApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IConfiguration _configuration;

    [BindProperty]
    public string EnvironmentName { get; set; } = "no data";

    [BindProperty]
    public string AspnetcoreEnvironment { get; set; } = "no data";
    
    [BindProperty]
    public string ConnectionString { get; set; } = "no data";

    public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public void OnGet()
    {
        EnvironmentName = Environment.GetEnvironmentVariable("HOST_NAME") ?? "no data [HOST_NAME]";
        AspnetcoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "no data [ASPNETCORE_ENVIRONMENT]";
        ConnectionString = _configuration["ConnectionStrings:DefaultConnection"] ?? "no data [Connection string]";

    }
}