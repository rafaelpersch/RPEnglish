using System.Text.Json;
using Microsoft.AspNetCore.Http;
using RPEnglish.MVC.Entities;

namespace RPEnglish.MVC.Tools
{
    public static class SessionManagement
    {
        public static void DeleteSession(HttpContext context)
        {
            context.Session.Remove("RPEnglish.MVC.Session");
            context.Session.Clear();
        }

        public static void CreateSession(HttpContext context, object obj)
        {
            context.Session.SetString("RPEnglish.MVC.Session", JsonSerializer.Serialize(obj));
        }

        public static User GetSession(HttpContext context)
        {
            try
            {
                var objString = context.Session.GetString("RPEnglish.MVC.Session");

                if (string.IsNullOrEmpty(objString))
                {
                    return null;
                }

                return JsonSerializer.Deserialize<User>(objString);
            }
            catch
            {
                return null;
            }
        }
    }
}
