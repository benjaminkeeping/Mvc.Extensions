using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Mvc.Extensions
{
    public static class FormExtensions
    {
        public static MvcForm BeginFormWithErrors<T>(this HtmlHelper<T> htmlHelper, object htmlAttributes)
        {
            return htmlHelper.BeginFormWithErrors(htmlHelper.ViewContext.HttpContext.Request.Url.PathAndQuery,
                                                  FormMethod.Post, htmlAttributes);
        }

        public static MvcForm BeginFormWithErrors<T>(this HtmlHelper<T> htmlHelper)
        {
            return htmlHelper.BeginFormWithErrors(htmlHelper.ViewContext.HttpContext.Request.Url.PathAndQuery,
                                                  FormMethod.Post, new RouteValueDictionary());
        }

        public static MvcForm BeginFormWithErrors<T>(this HtmlHelper<T> htmlHelper, string formAction)
        {
            return htmlHelper.BeginFormWithErrors(formAction, FormMethod.Post, new RouteValueDictionary());
        }

        public static MvcForm BeginFormWithErrors<T>(this HtmlHelper<T> htmlHelper, string formAction,
                                                     FormMethod formMethod)
        {
            return htmlHelper.BeginFormWithErrors(formAction, formMethod, new RouteValueDictionary());
        }

        public static MvcForm BeginFormWithErrors<T>(this HtmlHelper<T> htmlHelper, string formAction,
                                                     FormMethod formMethod, object htmlAttributes)
        {
            return htmlHelper.BeginFormWithErrors(formAction, formMethod, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcForm BeginFormWithErrors<T>(this HtmlHelper<T> htmlHelper, string formAction,
                                                     FormMethod formMethod, IDictionary<string, object> htmlAttributes)
        {
            var response = htmlHelper.ViewContext.HttpContext.Response;
            response.Cache.SetExpires(System.DateTime.UtcNow.AddDays(-1));
            response.Cache.SetValidUntilExpires(false);
            response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();

            var tagBuilder = new TagBuilder("form");

            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("action", formAction);
            tagBuilder.MergeAttribute("method", HtmlHelper.GetFormMethodString(formMethod), true);

            htmlHelper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            htmlHelper.ViewContext.Writer.Write(htmlHelper.FormErrors());
            return new MvcForm(htmlHelper.ViewContext);
        }

        public static MvcForm WithErrors<T>(this HtmlHelper<T> htmlHelper)
        {
            return new MvcForm(htmlHelper.ViewContext);
        }

        public static MvcHtmlString FormErrors<T>(this HtmlHelper<T> htmlHelper)
        {
            var methods = typeof(T).GetProperties().Select(x => x.Name);
            var errorNamesNotOnModel = htmlHelper.ViewData.ModelState.Keys.Where(x => !methods.Contains(x));

            var liHtml = new StringBuilder();
            foreach (var errorName in errorNamesNotOnModel)
            {
                htmlHelper.ViewData.ModelState[errorName].Errors.ToList().ForEach(
                    x =>
                        liHtml.Append(
                            string.Format(
                                "<li><span class=\"label label-warning\">Ooops!</span><span> {0}</span></li>",
                                x.ErrorMessage)));
            }
            return liHtml.Length == 0
                ? new MvcHtmlString("")
                : new MvcHtmlString(string.Format("<ul class=\"unstyled\">{0}</ul>", liHtml));
        }

    }
}