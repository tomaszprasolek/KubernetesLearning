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

    [BindProperty] 
    public string SomeSecretValue { get; set; } = "no data";

    [BindProperty]
    public string Feature1 { get; set; } = "no data";

    [BindProperty]
    public string Feature2 { get; set; } = "no data";

    public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public void OnGet()
    {
        // From deployment
        EnvironmentName = Environment.GetEnvironmentVariable("HOST_NAME") ?? "no data [HOST_NAME]";
        AspnetcoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "no data [ASPNETCORE_ENVIRONMENT]";

        // Secrets
        ConnectionString = _configuration["ConnectionStrings:DefaultConnection"] ?? "no data [Connection string]";
        SomeSecretValue = _configuration["SecretValue"] ?? "no data [SecretValue]";

        // Config map
        Feature1 = _configuration["FeatureNr1"] ?? "no data [Feature1]";
        Feature2 = _configuration["FeatureNr2"] ?? "no data [Feature2]";
    }
}