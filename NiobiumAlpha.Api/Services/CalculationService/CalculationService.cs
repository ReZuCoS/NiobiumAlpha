using NiobiumAlpha.Api.Models;

namespace NiobiumAlpha.Api.Services.CalculationService;

public interface ICalculationService {
    public Task<CalculationResult> Calculate(CalculationQuery query);
}
