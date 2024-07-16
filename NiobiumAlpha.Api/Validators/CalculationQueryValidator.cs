using System.Text.RegularExpressions;
using FluentValidation;
using NiobiumAlpha.Api.Models;

namespace NiobiumAlpha.Api.Validators;

public class CalculationQueryValidator : AbstractValidator<CalculationQuery>
{
    public CalculationQueryValidator()
    {
        RuleFor(e => e.Query)
            .NotEmpty()
            .Must(IsValidExpression)
            .WithMessage("Expression is invalid")
            .Must(ValidateCaracters)
            .WithMessage("Used unsupporded caracters")
            .Must(ValidateBrackets)
            .WithMessage("Brackets must be balanced");
    }

    private static bool IsValidExpression(string expression)
    {
        var lastWasOperator = true;
        var hasOperator = false;
        var inNumber = false;

        foreach (char ch in expression)
        {
            switch (ch)
            {
                case '(':
                case ')':
                case var _ when char.IsWhiteSpace(ch):
                    continue;

                case var _ when char.IsDigit(ch):
                    lastWasOperator = false;
                    inNumber = true;
                    break;

                case '.':
                    if (!inNumber) 
                    {
                        return false; // Точка не может быть первым символом числа
                    }
                    break;

                case '+':
                case '-':
                case '*':
                case '/':
                    if (!hasOperator) {
                        hasOperator = true;
                    }
                    if (lastWasOperator) {
                        return false; // Неверно, если два оператора идут подряд
                    }
                    lastWasOperator = true;
                    inNumber = false; // Число закончилось
                    break;

                default:
                    return false; // Неправильный символ
            }
        }

        // Последний символ должен быть операндом
        return hasOperator && !lastWasOperator;
    }

    private static bool ValidateCaracters(string expression) {
        var pattern = @"^[0-9+\-*/\(\)\s.]+$";
        return Regex.IsMatch(expression, pattern);
    }

    private static bool ValidateBrackets(string expression)
    {
        var stack = new Stack<char>();
        
        foreach (var ch in expression)
        {
            if (ch == '(')
            {
                stack.Push(ch);
            }
            else if (ch == ')')
            {
                if (stack.Count == 0)
                {
                    return false;
                }
                stack.Pop();
            }
        }

        return stack.Count == 0;
    }
}