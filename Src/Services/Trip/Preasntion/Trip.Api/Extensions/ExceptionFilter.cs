using BuildingBlocks.Exeptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

public sealed class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case ValidationException validationException:
                {
                    var errors = validationException.Errors
                        .GroupBy(error => error.PropertyName)
                        .ToDictionary(
                            group => group.Key,
                            group => group
                                .Select(error => error.ErrorMessage)
                                .Distinct()
                                .ToArray());

                    context.Result = new BadRequestObjectResult(
                        new ValidationProblemDetails(errors)
                        {
                            Title = "One or more validation errors occurred.",
                            Status = StatusCodes.Status400BadRequest
                        });

                    context.ExceptionHandled = true;
                    break;
                }

            case NotFoundException notFoundException:
                {
                    context.Result = new NotFoundObjectResult(new
                    {
                        message = notFoundException.Message
                    });

                    context.ExceptionHandled = true;
                    break;
                }
        }
    }
}
