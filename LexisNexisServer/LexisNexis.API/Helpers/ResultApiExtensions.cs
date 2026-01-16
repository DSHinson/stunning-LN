using LexisNexis.Common.Result;

namespace LexisNexis.API.Helpers
{
    public static class ResultApiExtensions
    {
        public static IResult ToApiResponse<T>(this Result<T> result)
        {
            //Pattern matching helper to convert Result<T> to IResult for API responses
            return result switch
            {
                Result<T>.Success success => Results.Ok(success.Data),
                Result<T>.Failure failure => Results.NotFound(new { message = failure.FailureMessage }),
                _ => Results.Problem("Unknown error")
            };
        }

        public static IResult ToApiResponse(this Result result, string SuccessMessage)
        {
            //Pattern matching helper to convert Result<T> to IResult for API responses
            return result switch
            {
                Result.Success success => Results.Ok(SuccessMessage),
                Result.Failure failure => Results.NotFound(new { message = failure.FailureMessage }),
                _ => Results.Problem("Unknown error")
            };
        }
    }

}
