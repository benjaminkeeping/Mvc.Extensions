using System.Linq;
using System.Text;
using System.Web.Mvc;
using Restful.Wiretypes;

namespace Mvc.Extensions
{
    public static class PaginationExtensions
    {

        public static MvcHtmlString Pagination<T>(this HtmlHelper htmlHelper, Page<T> page)
        {
            var buffer = new StringBuilder();
            buffer.Append("<ul class=\"pagination\">");
            buffer.Append(string.Join("",
                                      page.Links.Where(x => x.Title.ToLower() != "self").Select(
                                          x =>
                                              string.Format("<li{0}>{1}</li>", GetPageElementClass(x),
                                                            htmlHelper.Link(x)))));
            buffer.Append("</ul>");
            return new MvcHtmlString(buffer.ToString());
        }

        public static MvcHtmlString AjaxedPagination<T>(this HtmlHelper htmlHelper, Page<T> page, string baseUrl,
                                                        string functionName)
        {
            var buffer = new StringBuilder();
            buffer.Append("<ul class=\"pagination\">");
            buffer.Append(string.Format("<a href=\"javascript:{0}('{1}')\">{2}</a>", functionName,
                                        page.FormatPrevious(baseUrl), "<<"));
            foreach (var pageNumber in page.PageNumbers)
            {
                buffer.Append(string.Format("<a href=\"javascript:{0}('{1}')\">{2}</a>", functionName,
                                            page.FormatPageNumber(pageNumber, baseUrl), pageNumber.ToString()));
            }
            buffer.Append(string.Format("<a href=\"javascript:{0}('{1}')\">{2}</a>", functionName,
                                        page.FormatNext(baseUrl), ">>"));
            buffer.Append("</ul>");
            return new MvcHtmlString(buffer.ToString());
        }

        static string GetPageElementClass(Link link)
        {
            if (!link.IsDisabled && !link.IsActive) return "";
            if (link.IsActive && link.IsDisabled) return " class=\"active disabled\"";
            if (link.IsActive) return " class=\"active\"";
            return " class=\"disabled\"";
        }

    }

}
