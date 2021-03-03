using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RPEnglish.MVC.Tools
{
    public class CustomAuthorizationFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool deleteSession = false;

            try
            {
                var account = SessionManagement.GetSession(context.HttpContext);

                if (account == null)
                {
                    deleteSession = true;
                }
            }
            catch
            {
                deleteSession = true;
            }

            if (deleteSession)
            {
                SessionManagement.DeleteSession(context.HttpContext);
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}