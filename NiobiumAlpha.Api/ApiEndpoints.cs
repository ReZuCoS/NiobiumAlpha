namespace NiobiumAlpha.Api;

public class ApiEndpoints
{
    private const string ApiBase = "api";
    
    public class Calculation
    {
        public const string Calculate = $"{ApiBase}/calculate";
        public const string GetProviders = $"{ApiBase}/get_providers";
    }
}
