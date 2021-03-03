using Microsoft.AspNetCore.Http;

namespace RPEnglish.MVC.Tools
{
    public static class CookieManagement
    {
        public static string Get(HttpContext context, string name)
        {
            try
            {
                if (context.Request.Cookies[name] == null)
                {
                    return string.Empty;
                }

                return context.Request.Cookies[name].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static void Delete(HttpContext context, string name)
        {
            context.Response.Cookies.Delete(name);
        }

        public static void Create(HttpContext context, string name, string value)
        {
            context.Response.Cookies.Append(name, value, new CookieOptions()
            {
                IsEssential = true,
            });
        }
    }
}