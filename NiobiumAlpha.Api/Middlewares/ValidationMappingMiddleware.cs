using FluentValidation;
using NiobiumAlpha.Api.Contracts.Responses;

namespace NiobiumAlpha.Api.Middlewares;

public class ValidationMappingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = 400;
            var validationFailureResponse = new ValidationFailureResponse
            {
                Errors = ex.Errors.Select(x => new ValidationResponse
                {
                    PropertyName = x.PropertyName,
                    Message = x.ErrorMessage
                })
            };
            
            await context.Response.WriteAsJsonAsync(validationFailureResponse);
        }
    }
}