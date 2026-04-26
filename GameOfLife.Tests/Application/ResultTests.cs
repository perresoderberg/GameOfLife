using FluentAssertions;
using GameOfLife.Application.Results;
using Xunit;

public class ResultTests
{
    [Fact]
    public void Success_ShouldSetIsSuccessTrue()
    {
        var result = Result<int>.Success(42);

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Success_ShouldContainValue()
    {
        var result = Result<string>.Success("hello");

        result.Value.Should().Be("hello");
    }

    [Fact]
    public void Success_ShouldHaveNoError()
    {
        var result = Result<int>.Success(1);

        result.Error.Code.Should().Be("None");
    }

    [Fact]
    public void Fail_ShouldSetIsFailureTrue()
    {
        var error = Error.Validation("fail");

        var result = Result<int>.Fail(error);

        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Fail_ShouldContainError()
    {
        var error = Error.Validation("Something went wrong");

        var result = Result<int>.Fail(error);

        result.Error.Should().Be(error);
    }

    [Fact]
    public void Fail_ShouldNotContainValue()
    {
        var result = Result<int>.Fail(Error.Validation("fail"));

        result.Invoking(r => r.Value)
            .Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Error_None_ShouldHaveEmptyMessage()
    {
        var error = Error.None();

        error.Code.Should().Be("None");
        error.Message.Should().BeEmpty();
    }

    [Fact]
    public void Error_Null_ShouldHaveCorrectCode()
    {
        var error = Error.Null("null error");

        error.Code.Should().Be("NullError");
        error.Message.Should().Contain("null");
    }

    [Fact]
    public void Error_Validation_ShouldHaveCorrectCode()
    {
        var error = Error.Validation("validation failed");

        error.Code.Should().Be("ValidationError");
    }

    [Fact]
    public void Error_Unexpected_ShouldHaveCorrectCode()
    {
        var error = Error.Unexpected("unexpected");

        error.Code.Should().Be("UnexpectedError");
    }

    [Fact]
    public void Error_ToString_ShouldContainCodeAndMessage()
    {
        var error = Error.Validation("oops");

        var result = error.ToString();

        result.Should().Contain("ValidationError");
        result.Should().Contain("oops");
    }
}