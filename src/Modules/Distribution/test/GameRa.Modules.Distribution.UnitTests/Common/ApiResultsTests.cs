using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Common.Presentation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameRa.Modules.Distribution.UnitTests.Common;

public class ApiResultsTests
{
    [Theory]
    [InlineData(ErrorType.Validation, StatusCodes.Status400BadRequest)]
    [InlineData(ErrorType.Problem, StatusCodes.Status400BadRequest)]
    [InlineData(ErrorType.NotFound, StatusCodes.Status404NotFound)]
    [InlineData(ErrorType.Conflict, StatusCodes.Status409Conflict)]
    [InlineData(ErrorType.Forbidden, StatusCodes.Status403Forbidden)]
    [InlineData(ErrorType.Failure, StatusCodes.Status500InternalServerError)]
    public void Problem_ShouldMapErrorTypeToStatusCode(ErrorType errorType, int statusCode)
    {
        Result result = Result.Failure(new Error("Test.Code", "Test description", errorType));

        IResult problem = ApiResults.Problem(result);

        ProblemHttpResult httpResult = problem.Should().BeOfType<ProblemHttpResult>().Subject;
        httpResult.StatusCode.Should().Be(statusCode);
    }

    [Fact]
    public void Problem_ShouldExposeCodeAndDescription_WhenForbidden()
    {
        Result result = Result.Failure(Error.Forbidden("Test.Forbidden", "Not allowed"));

        var httpResult = (ProblemHttpResult)ApiResults.Problem(result);

        httpResult.ProblemDetails.Title.Should().Be("Test.Forbidden");
        httpResult.ProblemDetails.Detail.Should().Be("Not allowed");
    }
}
