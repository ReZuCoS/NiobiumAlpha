using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
using NiobiumAlpha.Api.Models;

namespace NiobiumAlpha.Api.Services.CalculationService;

public class WolframCalculationService(
    IValidator<CalculationQuery> queryValidator,
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration) : ICalculationService
{
    public async Task<CalculationResult> Calculate(CalculationQuery query)
    {
        await queryValidator.ValidateAndThrowAsync(query);

        var appId = configuration.GetValue<string>("WolframApi:AppId");
        var requestUrl = $"/v2/query?input={Uri.EscapeDataString(query.Query)}&appid={appId}&output=json&includepodid=Result";

        var httpClient = httpClientFactory.CreateClient("WolframAlpha");
        var response = await httpClient.GetAsync(requestUrl);

        if (!response.IsSuccessStatusCode)
        {
            throw new ValidationException([
                new ValidationFailure(nameof(query.Query), "Expression cannot be evaluated")
            ]);
        }
    
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var wolframResult = JsonSerializer.Deserialize<WolframAlphaResult>(jsonResponse);
        
        var result = wolframResult?.QueryResult?.Pods?[0].Subpods?[0].Plaintext;
        
        return new CalculationResult(result!);
    }
}
