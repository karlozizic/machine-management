using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace MachineManagement.API.Result;

public static class ResultExtensions
{
    public static ActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return new NoContentResult();
        }

        return MapErrorToActionResult(result.Error);
    }

    public static ActionResult<T> ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(result.Value);
        }

        return MapErrorToActionResult(result.Error);
    }

    private static ActionResult MapErrorToActionResult(Error error)
    {
        return error.Code switch
        {
            "ValidationError" or "BadRequest" => new BadRequestObjectResult(error),
            "NotFound" => new NotFoundObjectResult(error),
            "Unauthorized" => new UnauthorizedObjectResult(error),
            "Forbidden" => new ForbidResult(),
            "Conflict" => new ConflictObjectResult(error),
            "UnprocessableEntity" => new UnprocessableEntityObjectResult(error),
            "TooManyRequests" => new StatusCodeResult((int)HttpStatusCode.TooManyRequests),
            "InternalServerError" => new ObjectResult(error) { StatusCode = StatusCodes.Status500InternalServerError },
            _ => new ObjectResult(error) { StatusCode = StatusCodes.Status500InternalServerError }
        };
    }
}