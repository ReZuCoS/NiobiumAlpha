using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using NiobiumAlpha.Api.Contracts.Responses;
using NiobiumAlpha.Api.Mappings;
using NiobiumAlpha.Api.Models;
using NiobiumAlpha.Api.Services.CalculationService;

namespace NiobiumAlpha.Api.Endpoints.Calculate;

public static class CalculateEndpoint
{
    private const string Name = "Calculate";

    public static IEndpointRouteBuilder MapCalculate(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Calculation.Calculate, Calculate)
            .WithName(Name)
            .Produces<CalculationResponse>(StatusCodes.Status200OK)
            .Produces<ValidationResponse>(StatusCodes.Status400BadRequest);

        return app;
    }

    /// <summary>
    /// Solve using provider
    /// </summary>
    /// <param name="query">Expression</param>
    /// <param name="providerKey">Provider selection</param>
    /// <param name="serviceProvider"></param>
    /// <returns>Expression result</returns>
    /// <exception cref="ValidationException">Throws on invalid operation</exception>
    private static async Task<CalculationResponse> Calculate(
        [FromQuery] string query,
        [FromServices] IServiceProvider serviceProvider,
        [FromQuery] string providerKey = ApiKeyedServices.Calculation.Niobium)
    {
        ICalculationService calculationService;
        
        try
        {
            var serviceFactory = serviceProvider.GetRequiredService<Func<string, ICalculationService>>();
            calculationService = serviceFactory(providerKey);
        }
        catch (Exception)
        {
            throw new ValidationException([
                new ValidationFailure("", "Service not found")
            ]);
        }

        var result = await calculationService.Calculate(new CalculationQuery(query));

        return result.MapToResponse();
    }
}
