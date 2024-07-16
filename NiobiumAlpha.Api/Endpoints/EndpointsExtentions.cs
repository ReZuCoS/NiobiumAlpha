using NiobiumAlpha.Api.Endpoints.Calculate;
using NiobiumAlpha.Api.Endpoints.GetProviders;

namespace NiobiumAlpha.Api.Endpoints;

public static class EndpointsExtentions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetProviders();
        app.MapCalculate();
        
        return app;
    }
}
