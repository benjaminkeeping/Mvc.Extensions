using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Restful.Wiretypes;

namespace Mvc.Extensions
{
    public static class MvcExtensions
    {
        public static void AddErrors(this ModelStateDictionary modelState, IEnumerable<Error> errors)
        {
            errors.ToList().ForEach(err =>
            {
                if (err != null && err.Key != null && err.Value != null)
                    modelState.AddModelError(err.Key, err.Value);
                else
                {
                    if (err == null)
                        modelState.AddModelError("Unknown", "Error was null (something really bad probably happened)");
                    else if (err.Key == null && err.Value != null)
                        modelState.AddModelError("Unknown", err.Value);
                    else if (err.Key != null && err.Value == null)
                        modelState.AddModelError(err.Key, "Error value was null (something really bad probably happened)");
                    else
                        modelState.AddModelError("Unknown", "Error was null (something really bad probably happened)");

                }

            });
        }

        public static ActionResult WithModelErrors(this ActionResult result, ModelStateDictionary modelState, IEnumerable<Error> errors)
        {
            modelState.AddErrors(errors);
            return result;
        }

        public static ActionResult WithFlash(this ActionResult result, Controller controller, string message)
        {
            controller.TempData["Message"] = message;
            return result;
        }

        public static ActionResult WithNoCache(this ActionResult result)
        {
            HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache");
            return result;
        }

        public static ActionResult WithCookie(this ActionResult result, string cookieName, string cookieValue)
        {
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(cookieName, cookieValue));
            return result;
        }

    }
}