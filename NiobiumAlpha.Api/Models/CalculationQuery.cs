namespace NiobiumAlpha.Api.Models;

public class CalculationQuery(string query)
{
    public string Query { get; init; } = query;
}