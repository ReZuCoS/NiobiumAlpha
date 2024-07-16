using NiobiumAlpha.Api.Contracts.Responses;
using NiobiumAlpha.Api.Models;

namespace NiobiumAlpha.Api.Mappings;

public static class ContractMappings
{
    public static CalculationResponse MapToResponse(this CalculationResult calculationResult)
    {
        return new CalculationResponse
        {
            Result = calculationResult.Result
        };
    }
}