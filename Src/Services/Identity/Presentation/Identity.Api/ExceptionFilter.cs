using BuildingBlocks.Exeptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is NotFoundException )
        {
            context.Result = new NotFoundObjectResult(new { message = context.Exception.Message});
            context.ExceptionHandled = true;
        }
        if (context.Exception is UserNotFoundException)
        {
            context.Result = new NotFoundObjectResult(new { message = context.Exception.Message });
            context.ExceptionHandled = true;
        }
    }
}