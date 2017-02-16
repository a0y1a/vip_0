using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using System.Web.WebPages;

namespace KeKeSoftPlatform.Common
{
    public static partial class BootstrapHtmlHelperExtension
    {
        public static MvcHtmlString Icon(this HtmlHelper html, string iconType)
        {
            var span = new TagBuilder("span");
            span.AddCssClass(iconType);
            return new MvcHtmlString(span.ToString());
        }

        public static MvcHtmlString Divider(this HtmlHelper html)
        {
            var li=new TagBuilder("li");
            li.AddCssClass("divider");
            return new MvcHtmlString(li.ToString());
        }

        public static MvcHtmlString DropDownButtonGroup(this HtmlHelper html,string title, params MvcHtmlString[] item)
        {
            var group = new TagBuilder("div");
            group.AddCssClass("btn-group");

            var button = new TagBuilder("button");
            button.AddCssClass("btn btn-default btn-xs dropdown-toggle");
            button.Attributes["type"] = "button";
            button.Attributes["data-toggle"] = "dropdown";
            button.InnerHtml += title + " <span class=\"caret\"></span>";

            var itemBox = new TagBuilder("ul");
            itemBox.AddCssClass("dropdown-menu pull-right");
            itemBox.Attributes["role"] = "menu";

            if (item != null && item.Any())
            {
                foreach (var m in item)
                {
                    if (m == null)
                    {
                        continue;
                    }
                    if (item.ToString().StartsWith("<li"))
                    {
                        itemBox.InnerHtml += m;
                    }
                    else
                    {
                        var itemWrapper = new TagBuilder("li");
                        itemWrapper.InnerHtml += m.ToHtmlString();
                        itemBox.InnerHtml += itemWrapper.ToString();
                    }
                }
            }

            group.InnerHtml += button.ToString() + itemBox.ToString();
            return new MvcHtmlString(group.ToString());
        }
    }
}
