using Microsoft.AspNetCore.Mvc;

namespace Application.Utils
{
    internal interface IRazorViewEngine
    {
        object FindView(ActionContext actionContext, string viewName, bool v);
    }
}