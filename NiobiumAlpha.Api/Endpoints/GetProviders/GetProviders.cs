using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NiobiumAlpha.Api.Contracts.Responses;

namespace NiobiumAlpha.Api.Endpoints.GetProviders;

public static class GetProvidersEndpoint
{
    private const string Name = "GetProviders";

    public static IEndpointRouteBuilder MapGetProviders(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Calculation.GetProviders, GetProviders)
            .WithName(Name)
            .Produces<ProvidersResponse>(StatusCodes.Status200OK);

        return app;
    }

    /// <summary>
    /// Returns calculation providers
    /// </summary>
    private static async Task<ProvidersResponse> GetProviders(
        [FromServices] IServiceProvider serviceProvider)
    {
        return new ProvidersResponse
        {
            Providers = ApiKeyedServices.Calculation.GetServices()
        };
    }
}
