using NiobiumAlpha.Api.Services.CalculationService;

namespace NiobiumAlpha.Api;

public class ApiKeyedServices
{
    public class Calculation {
        public const string Niobium = nameof(NiobiumCalculationService);
        public const string Wolfram = nameof(WolframCalculationService);

        public static string[] GetServices()
        {
            return [Niobium, Wolfram];
        }
    }
}
