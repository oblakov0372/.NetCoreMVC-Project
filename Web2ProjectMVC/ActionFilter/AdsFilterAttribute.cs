using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Web2ProjectMVC.Entities;
using Web2ProjectMVC.ExtentionMethods;

namespace Web2ProjectMVC.ActionFilter
{
    public class AdsFilterAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            User loggedUser = context.HttpContext.Session.GetObject<User>("loggedUser");
            if (loggedUser == null || loggedUser.Role == Roles.User)
                context.Result = new RedirectResult("/Ads/Index");
        }
    }
}
