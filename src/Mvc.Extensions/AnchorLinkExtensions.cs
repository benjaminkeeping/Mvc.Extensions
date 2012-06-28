using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Restful.Wiretypes;

namespace Mvc.Extensions
{
    public static class AnchorLinkExtensions
    {
        public static MvcHtmlString Links(this HtmlHelper htmlHelper, IEnumerable<Link> links, bool showSelf)
        {
            if (links == null) return new MvcHtmlString("");
            var buffer = new StringBuilder();
            buffer.Append("<ul class=\"unstyled\">");
            if (!showSelf)
            {
                links = links.Where(x => x.Title.ToLower() != "self");
            }
            buffer.Append(string.Join("", links.Select(x => string.Format("<li>{0}</li>", htmlHelper.Link(x)))));
            buffer.Append("</ul>");
            return new MvcHtmlString(buffer.ToString());
        }

        public static MvcHtmlString Links(this HtmlHelper htmlHelper, IEnumerable<Link> links)
        {
            return htmlHelper.Links(links, false);
        }

        public static MvcHtmlString Link(this HtmlHelper htmlHelper, Link link)
        {
            if (link == null) return new MvcHtmlString("");
            if(link.IsDisabled) return new MvcHtmlString(string.Format("<span>{0}</span>", link.Title));
            return new MvcHtmlString(string.Format("<a href=\"{0}\">{1}</a>", link.Href, link.Title));
        }


    }
}