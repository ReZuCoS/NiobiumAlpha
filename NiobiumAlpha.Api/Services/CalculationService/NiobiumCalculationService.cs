using System.Globalization;
using System.Text;
using FluentValidation;
using FluentValidation.Results;
using NiobiumAlpha.Api.Models;

namespace NiobiumAlpha.Api.Services.CalculationService;

public class NiobiumCalculationService(
    IValidator<CalculationQuery> queryValidator) : ICalculationService
{
    private static readonly Dictionary<char, int> Priority = new()
    {
        {'(', 0},
        {'+', 1}, {'-', 1},
        {'*', 2}, {'/', 2}
    };

    public async Task<CalculationResult> Calculate(CalculationQuery query)
    {
        await queryValidator.ValidateAndThrowAsync(query);
    
        try
        {
            var result = Calculate(query.Query);
            return new CalculationResult(result.ToString(CultureInfo.InvariantCulture));
        }
        catch (DivideByZeroException) {
            throw new ValidationException([
                new ValidationFailure(nameof(query.Query), "Trying to divide by zero")
            ]);
        }
        catch (InvalidOperationException) {
            throw new ValidationException([
                new ValidationFailure(nameof(query.Query), "Expression cannot be evaluated")
            ]);
        }
    }

    /// <summary>
    /// Calculates infix notation expression (addition, subtraction, multiplication, division)
    /// by converting it to a Reverse Polish notation
    /// </summary>
    /// <param name="expression">Infix notation expression</param>
    /// <returns>Calculation result</returns>
    /// <exception cref="InvalidOperationException">Throws on invalid operation</exception>
    private static double Calculate(string expression)
    {
        Stack<double> result = new();
        Stack<char> operators = new();

        for (var i = 0; i < expression.Length; i++)
        {
            var ch = expression[i];

            switch (ch)
            {
                case var _ when char.IsWhiteSpace(ch):
                    continue;

                case var _ when char.IsDigit(ch):
                    var number = GetStringNumber(expression, ref i);
                    result.Push(Convert.ToDouble(number));
                    break;

                case '(':
                    operators.Push(ch);
                    break;

                case ')':
                    while (operators.Count > 0 && operators.Peek() != '(')
                    {
                        var operation = operators.Pop();
                        var second = result.Pop();
                        var first = result.Pop();
                        result.Push(ExecuteOperation(first, second, operation));
                    }
                    operators.Pop();
                    break;

                default:
                    while (operators.Count > 0 && (Priority[operators.Peek()] >= Priority[ch]))
                    {
                        var operation = operators.Pop();
                        var second = result.Pop();
                        var first = result.Pop();
                        result.Push(ExecuteOperation(first, second, operation));
                    }

                    operators.Push(ch);
                    break;
            }
        }

        while (operators.Count > 0)
        {
            var operation = operators.Pop();
            var second = result.Pop();
            var first = result.Pop();
            result.Push(ExecuteOperation(first, second, operation));
        }

        return result.Pop();
    }

    private static string GetStringNumber(string expression, ref int index)
    {
        var stringBuilder = new StringBuilder();
        var hasDecimalPoint = false;

        while (index < expression.Length)
        {
            var num = expression[index];

            if (char.IsDigit(num))
            {
                stringBuilder.Append(num);
            }
            else if (num == '.' && !hasDecimalPoint)
            {
                stringBuilder.Append(num);
                hasDecimalPoint = true;
            }
            else
            {
                index--;
                break;
            }

            index++;
        }

        return stringBuilder.ToString();
    }

    private static double ExecuteOperation(double first, double second, char operation) => operation switch
    {
        '+' => first + second,
        '-' => first - second,
        '*' => first * second,
        '/' => second != 0
            ? first / second
            : throw new DivideByZeroException(),
        _ => throw new InvalidOperationException()
    };
}

