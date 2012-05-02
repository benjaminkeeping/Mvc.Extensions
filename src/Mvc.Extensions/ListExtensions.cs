using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mvc.Extensions
{
    public static class ListExtensions
    {
        public static MvcHtmlString List(this HtmlHelper htmlHelper, IEnumerable<string> list)
        {
            return new MvcHtmlString(string.Format("<ul>{0}</ul>", string.Join("", list.Select(s => string.Format("<li>{0}</li>", (object) s)))));
        }
    }
}