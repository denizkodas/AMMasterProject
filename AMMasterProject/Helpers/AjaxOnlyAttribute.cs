using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AMMasterProject.Helpers
{
    //[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    //public class AjaxOnlyAttribute : Attribute, IActionConstraint
    //{
    //    public int Order => 0;

    //    public bool Accept(ActionConstraintContext context)
    //    {
    //        var headers = context.RouteContext.HttpContext.Request.Headers;

    //        if (headers["X-Requested-With"] == "XMLHttpRequest")
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            throw new HttpRequestException("This method can only be called via Ajax requests.");
    //        }
    //    }
    //}

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AjaxOnlyAttribute : Attribute, IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var headers = context.HttpContext.Request.Headers;

            if (headers["X-Requested-With"] != "XMLHttpRequest")
            {
                context.Result = new ContentResult
                {
                    Content = "This method can only be called via Ajax requests.",
                    StatusCode = 400, // or any other status code you prefer
                    ContentType = "text/plain"
                };
            }
            else
            {
                await next(); // Call the next filter or the action method
            }
        }
    }

}
