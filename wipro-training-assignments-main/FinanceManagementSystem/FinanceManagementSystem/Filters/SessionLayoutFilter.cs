using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FinanceManagementSystem.Filters
{
    public class SessionLayoutFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as Controller;
            if (controller != null)
            {
                var userId = context.HttpContext.Session.GetInt32("UserId");
                var username = context.HttpContext.Session.GetString("Username");
                var isAdmin = context.HttpContext.Session.GetString("IsAdmin") == "true";
                controller.ViewBag.IsLoggedIn = userId.HasValue;
                controller.ViewBag.Username = username ?? "";
                controller.ViewBag.IsAdmin = isAdmin;
            }

            await next();
        }
    }
}
