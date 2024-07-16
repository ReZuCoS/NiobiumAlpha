using FluentValidation;
using FluentValidation.Results;
using NiobiumAlpha.Api.Models;
using NiobiumAlpha.Api.Services.CalculationService;

namespace NiobiumAlpha.Tests.Services;

public class NiobiumCalculationServiceTests
{
    private readonly NiobiumCalculationService _service;
    private readonly Mock<IValidator<CalculationQuery>> _validatorMock = new();

    public NiobiumCalculationServiceTests()
    {
        _service = new NiobiumCalculationService(_validatorMock.Object);
    }
    
    [Fact]
    public async Task Calculate_ShouldCallValidator_AtMost_Twice()
    {
        // Arrange
        var query = new CalculationQuery("3 + 5");
        _validatorMock
            .Setup(v => v.ValidateAsync(query, default))
            .ReturnsAsync(new ValidationResult());

        // Act
        await _service.Calculate(query);

        // Assert
        _validatorMock.Verify(v => v.ValidateAsync(query, default), Times.AtMost(2));
    }
    
    [Theory]
    [InlineData("1 + 2", "3")]
    [InlineData("10+2*6", "22")]
    [InlineData("1.5 + 2.5", "4")]
    [InlineData("3.14 * 2", "6.28")]
    [InlineData("10.5 / 2", "5.25")]
    [InlineData("5.5 - 1.1", "4.4")]
    [InlineData("100 * 2 + 12", "212")]
    [InlineData("3*(2+ 1)/ 3 - 1", "2")]
    [InlineData("100 * (2 + 12)", "1400")]
    [InlineData("1 + 2 - 5 + (2 / 2)", "-1")]
    [InlineData("(2 + 3) * (4 - 1) / 3", "5")]
    [InlineData("100 * (2 + 12) / 14", "100")]
    public async Task Calculate_ShouldReturnCorrectResult_WhenExpressionIsValid(string expression, string expected)
    {
        // Arrange
        var query = new CalculationQuery(expression);
        _validatorMock
            .Setup(v => v.ValidateAsync(query, default))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await _service.Calculate(query);

        // Assert
        result.Result.Should().Be(expected);
    }

    [Theory]
    [InlineData("10 / 0")]
    [InlineData("(5 + 3) / 0")]
    [InlineData("8 / (4 - 4)")]
    [InlineData("12 - 7 * 2 / 0")]
    public async Task Calculate_ShouldThrowValidationException_WhenDivideByZero(string expression)
    {
        // Arrange
        var query = new CalculationQuery(expression);
        _validatorMock
            .Setup(v => v.ValidateAsync(query, default))
            .ReturnsAsync(new ValidationResult());

        // Act
        var act = async () => await _service.Calculate(query);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Trying to divide by zero*");
    }

    [Theory]
    [InlineData("10 /")]
    [InlineData("3 + +")]
    [InlineData("10 / * 2")]
    [InlineData("12 - (7 * 2")]
    [InlineData("12 - ) + 7 * 2")]
    public async Task Calculate_ShouldThrowValidationException_WhenExpressionCannotBeEvaluated(string expression)
    {
        // Arrange
        var query = new CalculationQuery(expression);
        _validatorMock
            .Setup(v => v.ValidateAsync(query, default))
            .ReturnsAsync(new ValidationResult());

        // Act
        var act = async () => await _service.Calculate(query);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Expression cannot be evaluated*");
    }
}